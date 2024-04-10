using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace easy_blazor_bulma;

/// <summary>
/// A link to use in a Navbar menu.
/// </summary>
/// <remarks>
/// There is 1 additional attribute that can be used: link-class. It will apply CSS classes to the resulting element as per its name.
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

    private readonly string[] Filter = new[] { "class", "link-class" };

    private string MainCssClass => string.Join(' ', "navbar-item", AdditionalAttributes.GetClass("class"));

    private string LinkCssClass
	{
		get
		{
			var css = "";

			if (string.IsNullOrWhiteSpace(Icon) == false)
				css += " ml-2";

			if (CompactDisplay && ChildContent != null)
				css += " is-hidden-touch is-hidden-desktop-only is-hidden-widescreen-only";

            return string.Join(' ', css.TrimStart(), AdditionalAttributes.GetClass("link-class"));
        }
	}
}
