## WyomingCrispAsrServer

This project is a Home Assistant–compatible Wyoming protocol server that works with Piper (V1) models. Tested on Windows.

### Implementation Details

The Wyoming server client-handling logic is located in the `WyomingTtsEventHandler` class.
The Piper model invocation logic is defined in the `Piper` class. It uses the `Phonemizer` class, which requires espeak-ng to be installed. The latest release can be found here: https://github.com/espeak-ng/espeak-ng/releases

### Configuration

The configuration must be specified in `appsettings.json`.

Example configuration:

```json
"WyomingPiperTtsServer": {
    "Host": "0.0.0.0",
    "Port": 10200
  },
  "Piper": {
    "EspeakDataDirectory": "C:\\Program Files\\eSpeak NG\\espeak-ng-data",
    "EspeakDllPath": "C:\\Program Files\\eSpeak NG\\libespeak-ng.dll",
    "Models": [
      {
        "Id": "ru_RU-ruslan-medium-int8",
        "Description": "ruslan (medium, INT8)",
        "Version": "1.0.0",
        "Languages": [ "ru" ],
        "Speakers": [],
        "Attribution": {
          "Name": "Credits",
          "Url": "https://ruslan-corpus.github.io"
        },
        "Path": "<path_to_ru_RU-ruslan-medium-int8.onnx>"
      }
    ]
  }
```

> [!NOTE]
> The sample configuration includes all fields that can be specified. Validation rules can be inferred from the model classes defined in `WyomingPiperTtsServerOptions.cs` and `PiperOptions.cs`.

> [!NOTE]
> The implementation also looks for a JSON configuration file. It should be placed alongside the model and follow the naming convention. For example, if the model is named `model.onnx`, then the configuration file should be named `model.onnx.json`.


Piper voices can be found here: https://huggingface.co/rhasspy/piper-voices/tree/main

### Performance Optimization

If the model performance is not sufficient and there is no low-end pre-trained model for your language, you can quantize a medium model using this Python script.
Quantized models may produce light background noise but with greatly improved inference speed (observed nearly 10× reduction on a medium model).
The JSON file with the model configuration should be copied and renamed to match the quantized model name.

```python
from onnxruntime.quantization import quantize_dynamic, QuantType

quantize_dynamic('<model>.onnx', 
                 '<model>-int8.onnx',
                 weight_type=QuantType.QUInt8)
```
