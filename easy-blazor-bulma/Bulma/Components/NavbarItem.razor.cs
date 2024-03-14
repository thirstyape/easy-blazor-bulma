using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// A link to use in a Navbar menu.
/// </summary>
/// <remarks>
/// <see href="https://bulma.io/documentation/components/navbar/">Bulma Documentation</see>
/// </remarks>
public partial class NavbarItem : ComponentBase
{
    /// <summary>
    /// An icon to display to the left of the title.
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }

	/// <summary>
	/// Specifies whether to make display items smaller on mobile resolutions.
	/// </summary>
	[Parameter]
	public bool CompactDisplay { get; set; }

	/// <inheritdoc cref="NavLink.Match" />
	[Parameter]
	public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;

	/// <summary>
	/// The content to display within the item.
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
			var css = "navbar-item";

			return string.Join(' ', css, CssClass);
		}
	}

	private string LinkCssClass
	{
		get
		{
			var css = "";

			if (string.IsNullOrWhiteSpace(Icon) == false)
				css += " ml-2";

			if (CompactDisplay && ChildContent != null)
				css += " is-hidden-touch is-hidden-desktop-only is-hidden-widescreen-only";

			return css.TrimStart();
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
