#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace WyomingPiperTtsServer.Models.Options;

internal sealed class PiperOptions
{
    [Required]
    public string EspeakDataDirectory { get; init; }

    [Required]
    public string EspeakDllPath { get; init; }

    [Required]
    [ValidateEnumeratedItems]
    public PiperModel[] Models { get; init; }
}

internal sealed class PiperModel
{
    [Required]
    public string Id { get; init; }

    public string? Description { get; init; }

    public string? Version { get; init; }

    [Required]
    public string[] Languages { get; init; }

    public string[]? Speakers { get; init; }

    [Required]
    [ValidateObjectMembers]
    public PiperModelAttribution Attribution { get; init; }

    [Required]
    public string Path { get; init; }
}

internal sealed class PiperModelAttribution
{
    [Required]
    public string Name { get; init; }

    [Required]
    public string Url { get; init; }
}

[OptionsValidator]
internal partial class ValidatePiperOptions : IValidateOptions<PiperOptions>
{
}
