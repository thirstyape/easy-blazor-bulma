using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// Creates an icon that displayed either a check or X based on the provided value.
/// </summary>
/// <remarks>
/// <see href="https://bulma.io/documentation/elements/icon/">Bulma Documentation</see>
/// </remarks>
public partial class BooleanIcon : ComponentBase
{
	/// <summary>
	/// Displays a green checkbox when true or a red X when false.
	/// </summary>
	[Parameter]
	public bool Value { get; set; }

	/// <summary>
	/// Any additional attributes applied directly to the component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object>? AdditionalAttributes { get; set; }

	private string FullCssClass(string text) => CssClass == null ? $"material-icons {text}" : string.Join(' ', $"material-icons {text}", CssClass);

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
