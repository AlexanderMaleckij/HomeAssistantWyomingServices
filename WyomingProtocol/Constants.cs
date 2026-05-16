namespace WyomingProtocol;

internal class Constants
{
    public static class EventTypes
    {
        public const string Describe = "describe";
        public const string Info = "info";
        public const string Synthesize = "synthesize";
        public const string SynthesizeStart = "synthesize-start";
        public const string SynthesizeChunk = "synthesize-chunk";
        public const string SynthesizeStop = "synthesize-stop";
        public const string SynthesizeStopped = "synthesize-stopped";
        public const string Transcribe = "transcribe";
        public const string Transcript = "transcript";
        public const string AudioStart = "audio-start";
        public const string AudioChunk = "audio-chunk";
        public const string AudioStop = "audio-stop";
        public const string Recognize = "recognize";
        public const string Intent = "intent";
        public const string NotRecognized = "not-recognized";
        public const string IntentsStart = "intents-start";
        public const string IntentsStop = "intents-stop";
        public const string Handled = "handled";
        public const string NotHandled = "not-handled";
        public const string HandledStart = "handled-start";
        public const string HandledChunk = "handled-chunk";
        public const string HandledStop = "handled-stop";
    }
}
