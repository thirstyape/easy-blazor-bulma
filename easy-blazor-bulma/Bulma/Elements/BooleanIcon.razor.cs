using Microsoft.AspNetCore.Components;

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

    private readonly string[] Filter = new[] { "class" };

    private string MainCssClass
	{
		get
		{
			var css = "material-icons";

			if (Value)
				css += " has-text-success";
			else
				css += " has-text-danger";

			return string.Join(' ', css, AdditionalAttributes.GetClass("class"));
		}
	}

	private string Icon => Value ? "check_circle" : "cancel";
}
