using System.Buffers;
using System.Text.Json;

namespace WyomingProtocol.Extensions;

internal static class JsonElementExtensions
{
    extension(JsonElement source)
    {
        public JsonElement Merge(JsonElement @override)
        {
            var buffer = new ArrayBufferWriter<byte>();
            using var writer = new Utf8JsonWriter(buffer);

            writer.WriteStartObject();

            // Write all override properties first (they win)
            foreach (var prop in @override.EnumerateObject())
            {
                prop.WriteTo(writer);
            }

            // Write base properties only if not already in override
            var overrideKeys = @override.EnumerateObject()
                .Select(p => p.Name)
                .ToHashSet(StringComparer.Ordinal);

            foreach (var prop in source.EnumerateObject())
            {
                if (!overrideKeys.Contains(prop.Name))
                {
                    prop.WriteTo(writer);
                }
            }

            writer.WriteEndObject();
            writer.Flush();

            return JsonDocument.Parse(buffer.WrittenMemory).RootElement;
        }
    }
}
