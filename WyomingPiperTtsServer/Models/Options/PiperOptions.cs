#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace WyomingPiperTtsServer.Models.Options;

internal sealed class PiperOptions
{
    [Required]
    public string EspeakDataDirectory { get; set; }

    [Required]
    public string EspeakDllPath { get; set; }

    [Required]
    [ValidateEnumeratedItems]
    public PiperModel[] Models { get; set; }
}

internal sealed class PiperModel
{
    [Required]
    public string Id { get; set; }

    public string? Description { get; set; }

    public string? Version { get; set; }

    [Required]
    public string[] Languages { get; set; }

    public string[]? Speakers { get; set; }

    [Required]
    [ValidateObjectMembers]
    public PiperModelAttribution Attribution { get; set; }

    [Required]
    public string Path { get; set; }
}

internal sealed class PiperModelAttribution
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Url { get; set; }
}

[OptionsValidator]
internal partial class ValidatePiperOptions : IValidateOptions<PiperOptions>
{
}
