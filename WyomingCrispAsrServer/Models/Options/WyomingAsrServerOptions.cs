#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace WyomingCrispAsrServer.Models.Options.WyomingAsrServer;

internal sealed class WyomingAsrServerOptions
{
    [Required]
    public required string Host { get; set; }

    [Required]
    public required int Port { get; set; }

    [Required]
    public required string CrispAsrPath { get; set; }

    public string? FallbackLanguage { get; set; }

    [Required]
    [ValidateEnumeratedItems]
    public required WyomingAsrServerModel[] Models { get; set; }
}

internal sealed class WyomingAsrServerModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    public WyomingAsrServerModelAttribution Attribution { get; set; }

    public string? Description { get; set; }

    public string? Version { get; set; }

    [Required]
    public string[] Languages { get; set; }

    [Required]
    public string ModelFile { get; set; }

    public bool SupportsLanguageAutoDetect { get; set; }
}

internal record WyomingAsrServerModelAttribution([Required] string Name, [Required] string Url);

[OptionsValidator]
internal partial class ValidateWyomingAsrServerOptions : IValidateOptions<WyomingAsrServerOptions>
{
}
