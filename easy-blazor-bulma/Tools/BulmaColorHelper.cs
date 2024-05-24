using easy_core;
using System.ComponentModel.DataAnnotations;

namespace easy_blazor_bulma;

/// <summary>
/// Contains methods to assist with use of the colors within Bulma.
/// </summary>
public static class BulmaColorHelper
{
    /// <summary>
	/// Converts the specified Bulma color to its RGB hex value.
	/// </summary>
	/// <param name="color">The color to convert</param>
	/// <exception cref="ArgumentException"></exception>
	public static string GetHexColor(BulmaColors color) => color switch
    {
        BulmaColors.Default => "#0A0A0A",
        BulmaColors.Green => "#48C78E",
        BulmaColors.Yellow => "#FFE08A",
        BulmaColors.Red => "#F14668",
        BulmaColors.Turquoise => "#00D1B2",
        BulmaColors.Purple => "#B86BFF",
        BulmaColors.Blue => "#485FC7",
        BulmaColors.Cyan => "#3E8ED0",
        BulmaColors.Dark => "#0A0A0A",
        BulmaColors.Light => "#FFFFFF",
        _ => throw new ArgumentException(null, nameof(color))
    };

    /// <summary>
    /// Converts the specified Bulma color to its text CSS class.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <param name="suffix">Specifies the suffix to apply to the CSS class.</param>
    /// <remarks>
    /// Valid suffixes include:
    ///
    ///     light,
    ///     dark
    /// </remarks>
    public static string GetTextCss(BulmaColors color, string? suffix = null) => GetColorCss(color, "has-text", suffix);

    /// <summary>
    /// Converts the specified Bulma color to its background CSS class.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <param name="suffix">Specifies the suffix to apply to the CSS class.</param>
    /// <remarks>
    /// Valid suffixes include:
    ///
    ///     light,
    ///     dark
    /// </remarks>
    public static string GetBackgroundCss(BulmaColors color, string? suffix = null) => GetColorCss(color, "has-background", suffix);

    /// <summary>
    /// Converts the specified Bulma color to its CSS class.
    /// </summary>
    /// <param name="color">The color to convert</param>
    /// <param name="prefix">Specifies the prefix to apply to the CSS class.</param>
    /// <param name="suffix">Specifies the suffix to apply to the CSS class.</param>
    /// <remarks>
    /// Valid prefixes include:
    ///
    ///     is,
    ///     has-text,
    ///     has-background
    ///
    /// Valid suffixes include:
    ///
    ///     light,
    ///     dark
    /// </remarks>
    public static string GetColorCss(BulmaColors color, string prefix = "is", string? suffix = null) => string.IsNullOrWhiteSpace(suffix) ?
        $"{prefix}-{color.GetValueAttribute<BulmaColors, DisplayAttribute>()?.GetShortName()}" :
        $"{prefix}-{color.GetValueAttribute<BulmaColors, DisplayAttribute>()?.GetShortName()}-{suffix}";

    /// <summary>
    /// Returns the enum value for the provided text color string.
    /// </summary>
    /// <param name="color">The color to parse into its enum value.</param>
    public static BulmaColors GetBulmaColor(string color) => Enum.GetValues<BulmaColors>().SingleOrDefault(x => x.GetValueAttribute<BulmaColors, DisplayAttribute>()!.GetShortName() == color.Split('-').Last());

    /// <summary>
    /// Returns the display name of the provided color.
    /// </summary>
    /// <param name="color">The color to return the display name for.</param>
    public static string GetColorName(BulmaColors color) => color.GetValueAttribute<BulmaColors, DisplayAttribute>()!.GetName()!;
}
