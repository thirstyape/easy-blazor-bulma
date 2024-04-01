using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// Creates an imposing hero banner to showcase something.
/// </summary>
/// <remarks>
/// <see href="https://bulma.io/documentation/layout/hero/">Bulma Documentation</see>
/// </remarks>
public partial class Hero : ComponentBase
{
	/// <summary>
	/// The text to use as the main title for the hero.
	/// </summary>
	[Parameter]
	public string? Title { get; set; }

	/// <summary>
	/// The text to use as the subtitle for the hero.
	/// </summary>
	[Parameter]
	public string? Subtitle { get; set; }

	/// <summary>
	/// Sets the color to use for the hero background.
	/// </summary>
	[Parameter]
	public BulmaColors Color { get; set; }

	/// <summary>
	/// The content to display in the top section of the hero.
	/// </summary>
	[Parameter]
	public RenderFragment? HeroHead { get; set; }

	/// <summary>
	/// The content to display in the main section of the hero.
	/// </summary>
	[Parameter]
	public RenderFragment? HeroBody { get; set; }

	/// <summary>
	/// The content to display in the bottom section of the hero.
	/// </summary>
	[Parameter]
	public RenderFragment? HeroFoot { get; set; }

	/// <summary>
	/// Fallback for content to display within the hero.
	/// </summary>
	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	/// <summary>
	/// Any additional attributes applied directly to the component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object>? AdditionalAttributes { get; set; }

	private string FullCssClass 
	{
		get
		{
			var css = "hero";

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
