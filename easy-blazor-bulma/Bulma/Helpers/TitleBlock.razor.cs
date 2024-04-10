using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace easy_blazor_bulma;

/// <summary>
/// Creates a styled block to display a title at the top of a page.
/// </summary>
/// <remarks>
/// There is 1 additional attribute that can be used: body-class. It will apply CSS classes to the resulting element as per its name.
/// </remarks>
public partial class TitleBlock : ComponentBase
{
	/// <summary>
	/// The text to display in the bar.
	/// </summary>
	[Required]
	[Parameter]
	public string Title { get; set; }

	/// <summary>
	/// Appends to the provided title in the browser tab name. Only applies when <see cref="UpdatePageTitle"/> is true.
	/// </summary>
	[Parameter]
	public string? ApplicationName { get; set; }

	/// <summary>
	/// An optional icon to display to the left of the title.
	/// </summary>
	[Parameter]
	public string? Icon { get; set; }

	/// <summary>
	/// An optional link to apply to the icon.
	/// </summary>
	[Parameter]
	public string? IconUrl { get; set; }

	/// <summary>
	/// Specifies whether the tiny sizing should be applied to the title bar.
	/// </summary>
	[Parameter]
	public bool IsTiny { get; set; } = true;

	/// <summary>
	/// Specifies whether the title bar text should be in bold.
	/// </summary>
	[Parameter]
	public bool IsBold { get; set; } = true;

	/// <summary>
	/// Specifies whether to update the browser tab name with the title.
	/// </summary>
	[Parameter]
	public bool UpdatePageTitle { get; set; } = true;

	/// <summary>
	/// Any additional attributes applied directly to the component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object>? AdditionalAttributes { get; set; }

	private readonly string[] Filter = new[] { "class", "body-class" };

	private string MainCssClass
	{
		get
		{
			var css = "hero is-primary";

			if (IsTiny)
				css += " is-tiny";

			if (IsBold)
				css += " is-bold";

			return string.Join(' ', css, AdditionalAttributes.GetClass("class"));
		}
	}

	private string BodyCssClass => string.Join(' ', "hero-body pl-4", AdditionalAttributes.GetClass("body-class"));
}
