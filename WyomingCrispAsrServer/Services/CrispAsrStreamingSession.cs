using System.Diagnostics;
using System.Text;

namespace WyomingCrispAsrServer.Services
{
    /// <summary>
    /// Reads a raw byte stream that contains ANSI escape sequences and CR/LF control characters,
    /// reconstructing only the final committed lines (terminated by \n).
    /// ESC[...K and \r are treated as "erase current line" signals.
    /// </summary>
    internal sealed class AnsiLineReader
    {
        private readonly List<byte> _lineBytes = new();
        private readonly Action<string> _onLineCommitted;

        public AnsiLineReader(Action<string> onLineCommitted)
        {
            _onLineCommitted = onLineCommitted;
        }

        public void Feed(Span<byte> buffer)
        {
            int i = 0;
            int count = buffer.Length;
            while (i < count)
            {
                byte b = buffer[i];

                if (b == 0x1B) // ESC
                {
                    i++;
                    if (i < count && buffer[i] == (byte)'[')
                    {
                        i++; // skip [
                        // skip parameter bytes (digits, semicolons)
                        while (i < count && buffer[i] != (byte)'K'
                                         && buffer[i] != (byte)'m'
                                         && buffer[i] != (byte)'A'
                                         && buffer[i] != (byte)'J')
                            i++;

                        if (i < count && buffer[i] == (byte)'K')
                            _lineBytes.Clear(); // ESC[K / ESC[2K — erase line

                        i++; // skip final command char
                    }
                }
                else if (b == (byte)'\r')
                {
                    // Carriage return — overwrite from line start
                    _lineBytes.Clear();
                    i++;
                }
                else if (b == (byte)'\n')
                {
                    // Newline — commit line
                    var line = Encoding.UTF8.GetString(_lineBytes.ToArray()).Trim();
                    if (!string.IsNullOrWhiteSpace(line))
                        _onLineCommitted(line);

                    _lineBytes.Clear();
                    i++;
                }
                else
                {
                    _lineBytes.Add(b);
                    i++;
                }
            }
        }

        public void Flush()
        {
            if (_lineBytes.Count == 0) return;

            var line = Encoding.UTF8.GetString(_lineBytes.ToArray()).Trim();
            if (!string.IsNullOrWhiteSpace(line))
                _onLineCommitted(line);

            _lineBytes.Clear();
        }
    }

    internal sealed class CrispAsrStreamingSession : IDisposable
    {
        private readonly Process _process;
        private readonly StringBuilder _errorBuffer = new();
        private readonly AnsiLineReader _outputReader;
        private readonly StringBuilder _outputBuffer;

        public CrispAsrStreamingSession(FileInfo crispAsr, string modelPath, string? language = null, int stepMs = 3600_000)
        {
            if (!Path.Exists(modelPath))
            {
                throw new FileNotFoundException("STT Model file not found.");
            }

            if (language is not null && language.Length > 3)
            {
                throw new ArgumentException("Language is not valid", nameof(language));
            }

            var processStartInfo = new ProcessStartInfo
            {
                FileName = crispAsr.FullName,
                Arguments = $"--stream -m \"{modelPath}\" {(language is not null ? $"-l {language}" : "")} -np true --stream-step {stepMs}",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            processStartInfo.Environment["PATH"] = $"{crispAsr.DirectoryName};";

            _process = new Process { StartInfo = processStartInfo };

            _outputBuffer = new StringBuilder();
            _outputReader = new AnsiLineReader((line) => _outputBuffer.Append(line));

            _process.OutputDataReceived += HandleCrispAsrData;
            _process.ErrorDataReceived += HandleCrispAsrErrorData;

            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
        }

        public ValueTask AddPcmDataAsync(byte[] data, CancellationToken cancellationToken)
        {
            return _process.StandardInput.BaseStream.WriteAsync(data, cancellationToken);
        }

        public async Task<string> GetTranscriptionAsync(CancellationToken cancellationToken)
        {
            await _process.StandardInput.BaseStream.FlushAsync(cancellationToken);

            _process.StandardInput.BaseStream.Close();

            await _process.WaitForExitAsync(cancellationToken);

            if (_process.ExitCode != 0)
            {
                throw new InvalidOperationException($"crispasr exited with code {_process.ExitCode}: {_errorBuffer}");
            }

            _outputReader.Flush();

            return _outputBuffer.ToString();
        }

        public void Dispose()
        {
            _process.OutputDataReceived -= HandleCrispAsrData;
            _process.ErrorDataReceived -= HandleCrispAsrErrorData;
            _process.Dispose();
        }

        private void HandleCrispAsrData(object sender, DataReceivedEventArgs e)
        {
            if (e.Data is not null)
            {
                _outputReader.Feed(Encoding.UTF8.GetBytes(e.Data));
            }
        }

        private void HandleCrispAsrErrorData(object sender, DataReceivedEventArgs e)
        {
            if (e.Data is not null)
            {
                _errorBuffer.AppendLine(e.Data);
            }
        }
    }
}
