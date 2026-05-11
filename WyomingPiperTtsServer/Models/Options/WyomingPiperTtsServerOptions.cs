#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace WyomingPiperTtsServer.Models.Options;

internal sealed class WyomingPiperTtsServerOptions
{
    [Required]
    public string Host { get; set; }

    [Required]
    public int Port { get; set; }
}

[OptionsValidator]
internal partial class ValidateWyomingPiperTtsServerOptions : IValidateOptions<WyomingPiperTtsServerOptions>
{
}
