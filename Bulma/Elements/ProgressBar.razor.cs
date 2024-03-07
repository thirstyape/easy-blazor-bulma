using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// Creates a progress bar that displays the provided value or a loading animation.
/// </summary>
/// <remarks>
/// <see href="https://bulma.io/documentation/elements/progress/">Bulma Documentation</see>
/// </remarks>
public partial class ProgressBar : ComponentBase
{
    /// <summary>
    /// The point to bring the bar to in relation to <see cref="Max"/>.
    /// </summary>
    [Parameter]
    public int? Current { get; set; }

    /// <summary>
    /// The value at which the bar will be completely filled.
    /// </summary>
    [Parameter]
    [Range(0, int.MaxValue)]
    public int Max { get; set; } = 100;

    /// <summary>
    /// Sets the fill color for the progress bar.
    /// </summary>
    [Parameter]
    public BulmaColors Color { get; set; }

    /// <summary>
	/// Any additional attributes applied directly to the component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string FullCssClass
    {
        get
        {
            var css = "progress";

            if (Color != BulmaColors.Default)
                css += ' ' + BulmaColorHelper.GetColorCss(Color);

            return string.Join(' ', css, CssClass);
        }
    }

    private string? CssClass
    {
        get
        {
            if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("class", out var css) && string.IsNullOrWhiteSpace(Convert.ToString(css, CultureInfo.InvariantCulture)) == false)
                return css.ToString();

            return null;
        }
    }
}
