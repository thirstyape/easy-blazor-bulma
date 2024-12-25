using System.ComponentModel.DataAnnotations;

namespace easy_blazor_bulma;

/// <summary>
/// Enum containing a list of available color options in Bulma.
/// </summary>
/// <remarks>
/// For more details see <see href="https://bulma.io/documentation/overview/colors/">Bulma Documentation</see>
/// </remarks>
public enum BulmaColors
{
    /// <summary>
    /// Default, varies depending on whether dark or light theme is selected.
    /// </summary>
    [Display(Name = "Default", ShortName = "default")]
    Default,

    /// <summary>
    /// Green or success.
    /// </summary>
    [Display(Name = "Green", ShortName = "success")]
    Green,

    /// <summary>
    /// Yellow or warning.
    /// </summary>
    [Display(Name = "Yellow", ShortName = "warning")]
    Yellow,

    /// <summary>
    /// Red or danger.
    /// </summary>
    [Display(Name = "Red", ShortName = "danger")]
    Red,

    /// <summary>
    /// Turquoise or primary.
    /// </summary>
    [Display(Name = "Turquoise", ShortName = "primary")]
    Turquoise,

    /// <summary>
    /// Purple or secondary.
    /// </summary>
    [Display(Name = "Purple", ShortName = "secondary")]
    Purple,

	/// <summary>
	/// Orange or tertiary.
	/// </summary>
	[Display(Name = "Orange", ShortName = "tertiary")]
	Orange,

    /// <summary>
    /// Blue or link.
    /// </summary>
    [Display(Name = "Blue", ShortName = "link")]
    Blue,

    /// <summary>
    /// Cyan or info.
    /// </summary>
    [Display(Name = "Cyan", ShortName = "info")]
    Cyan,

    /// <summary>
    /// Dark.
    /// </summary>
    [Display(Name = "Dark", ShortName = "dark")]
    Dark,

    /// <summary>
    /// Light.
    /// </summary>
    [Display(Name = "Light", ShortName = "light")]
    Light,

	/// <summary>
	/// Pink or highlight.
	/// </summary>
	[Display(Name = "Pink", ShortName = "highlight")]
	Pink
}
