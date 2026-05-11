## WyomingCrispAsrServer

This project is a Home Assistant–compatible Wyoming protocol server that calls the CrispASR executable. Tested on Windows.

> [!NOTE]
> Only streaming ASR models compatible with CrispASR are supported.

> [!TIP]
> The following command can be used to test the model: `ffmpeg -i "recording.wav" -f s16le -ar 16000 -ac 1 - | crispasr --stream -m "<path_to_your_model>" --stream-step 3600`
> `ffmpeg` can be installed from WinGet: `winget install --id=Gyan.FFmpeg -e`

### Implementation Details

The Wyoming server client-handling logic is located in the `WyomingAsrEventHandler` class.
The CrispASR invocation logic is used in the handler and located in the `CrispAsrStreamingSession` class.

### Configuration

The configuration must be specified in the `appsettings.json`.

Example configuration:

```json
{
  "WyomingCrispAsrServer": {
    "Host": "0.0.0.0",
    "Port": 10300,
    "CrispAsrPath": "<path_to_crispasr.exe>",
    //"FallbackLanguage": "en", // Not requred when all models support language auto-detect.
    "Models": [
      {
        "Name": "parakeet-tdt-0.6b-v3-q4_k",
        "Attribution": {
          "Name": "NVIDIA parakeet-tdt-0.6b-v3",
          "Url": "https://huggingface.co/nvidia/parakeet-tdt-0.6b-v3"
        },
        "Description": "parakeet-tdt-0.6b Q4_K Multilingual Speech-to-Text Model",
        "Version": "3.0.0",
        "Languages": [ "bg", "hr", "cs", "da", "nl", "en", "et", "fi", "fr", "de", "el", "hu", "it", "lv", "lt", "mt", "pl", "pt", "ro", "sk", "sl", "es", "sv", "ru", "uk" ],
        "ModelFile": "<path_to_parakeet-tdt-0.6b-v3-q4_k.gguf>",
        "SupportsLanguageAutoDetect": true
      }
    ]
  }
}
```

> [!NOTE]
> The sample configuration includes all fields that can be specified. Validation rules can be inferred from the model classes defined in `WyomingAsrServerOptions.cs`.
