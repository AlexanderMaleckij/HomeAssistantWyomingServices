using System.Runtime.InteropServices;
using System.Text;

namespace WyomingPiperTtsServer;

internal sealed class Phonemizer : IDisposable
{
    // ─── Constants from speak_lib.h ───────────────────────────────────────────
    private const int AUDIO_OUTPUT_SYNCHRONOUS = 2;
    private const int espeakCHARS_UTF8 = 1;
    private const int espeakINITIALIZE_PHONEME_IPA = 0x0002;
    private const int espeakPHONEMES_IPA = 0x02;   // bit 1 → IPA output
    // Separator character encoded in bits 8–23: e.g. space = 0x20 << 8
    // 0 means no separator

    // ─── P/Invoke declarations ─────────────────────────────────────────────────
    private const string DllName = "libespeak-ng.dll";

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern int espeak_Initialize(
        int output,
        int buflength,
        string? path,
        int options);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern int espeak_SetVoiceByName(string name);

    /// textptr is a pointer-to-pointer: IntPtr* in C#
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr espeak_TextToPhonemes(
        ref IntPtr textptr,
        int textmode,
        int phonememode);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int espeak_Terminate();

    // ─── Fields ───────────────────────────────────────────────────────────────
    private readonly string _defaultVoice;
    private bool _disposed;

    // ─── Constructor ──────────────────────────────────────────────────────────
    public Phonemizer(
        string defaultVoice = "en",
        string? espeakNgDataDirectory = null,   // parent dir that contains espeak-ng-data\
        string? espeakNgDllPath = null)          // used to load the DLL from a custom path
    {
        _defaultVoice = defaultVoice;

        // Optionally load the DLL from a non-default location before first P/Invoke
        if (!string.IsNullOrEmpty(espeakNgDllPath))
            NativeLibrary.Load(espeakNgDllPath);

        // espeakNgDataDirectory  →  parent folder that contains espeak-ng-data\
        // Pass null to use the compiled-in default.
        int sampleRate = espeak_Initialize(
            AUDIO_OUTPUT_SYNCHRONOUS,
            /*buflength*/ 0,
            espeakNgDataDirectory,
            espeakINITIALIZE_PHONEME_IPA);   // enables IPA output from the start

        if (sampleRate == -1)
            throw new Exception("eSpeak-NG failed to initialize. Check your data path.");
    }

    // ─── Public API ───────────────────────────────────────────────────────────
    public string Phonemize(
        string text,
        string? voice = null,
        bool noStress = false,
        bool keepLanguageFlags = true,
        string? punctuationSeparator = null,
        bool keepClauseBreakers = true,
        string? phonemeSeparator = null)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        voice ??= _defaultVoice;

        int rc = espeak_SetVoiceByName(voice);
        if (rc != 0)
            throw new Exception($"espeak_SetVoiceByName(\"{voice}\") failed with code {rc}.");

        // Build phonememode:
        //   bit 1  = 1  → IPA names
        //   bits 8-23   → separator char (0 = none)
        int phonemeMode = espeakPHONEMES_IPA;
        if (!string.IsNullOrEmpty(phonemeSeparator))
            phonemeMode |= (phonemeSeparator[0] << 8);

        // Pin the UTF-8 bytes so the native pointer stays valid across the loop
        byte[] utf8 = Encoding.UTF8.GetBytes(text + '\0');
        var result = new StringBuilder();

        unsafe
        {
            fixed (byte* pBytes = utf8)
            {
                // textptr points to the current position inside the UTF-8 buffer
                IntPtr textPtr = (IntPtr)pBytes;

                while (textPtr != IntPtr.Zero)
                {
                    IntPtr phonemePtr = espeak_TextToPhonemes(
                        ref textPtr,
                        espeakCHARS_UTF8,
                        phonemeMode);

                    if (phonemePtr == IntPtr.Zero)
                        break;

                    string? chunk = Marshal.PtrToStringUTF8(phonemePtr);
                    if (!string.IsNullOrEmpty(chunk))
                        result.Append(chunk);
                }
            }
        }

        return result.ToString();
    }

    // ─── IDisposable ──────────────────────────────────────────────────────────
    public void Dispose()
    {
        if (!_disposed)
        {
            espeak_Terminate();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    ~Phonemizer() => Dispose();
}
