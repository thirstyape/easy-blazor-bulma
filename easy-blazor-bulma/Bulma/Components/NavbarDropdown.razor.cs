using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// A dropdown to use in a Navbar menu to contain additional items.
/// </summary>
/// <remarks>
/// There is additional attribute that can be used: dropdown-class. It will apply CSS classes to the resulting element as per its name.
/// <see href="https://bulma.io/documentation/components/navbar/">Bulma Documentation</see>
/// </remarks>
public partial class NavbarDropdown : ComponentBase
{
	/// <summary>
	/// The name to display on the navbar header item.
	/// </summary>
	[Parameter]
	public string? DisplayText { get; set; }

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

	/// <summary>
	/// Specifies whether the dropdown should use the full screen width.
	/// </summary>
	[Parameter]
	public bool IsFullWidth { get; set; }

	/// <summary>
	/// The content to display within the dropdown.
	/// </summary>
	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	/// <summary>
	/// Any additional attributes applied directly to the component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object>? AdditionalAttributes { get; set; }

	private bool IsActive;

	private string FullCssClass
	{
		get
		{
			var css = "navbar-item has-dropdown is-hoverable";

			if (IsFullWidth)
				css += " is-mega";

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

			if (CompactDisplay && string.IsNullOrWhiteSpace(DisplayText) == false)
				css += " is-hidden-touch is-hidden-desktop-only is-hidden-widescreen-only";

			return css.TrimStart();
		}
	}

	private string DropdownCssClass
	{
		get
		{
			var css = "navbar-dropdown";

			if (IsActive)
				css += " is-active";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("dropdown-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
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

	private readonly string[] Filter = new[] { "class", "dropdown-class" };
	private IReadOnlyDictionary<string, object>? FilteredAttributes => AdditionalAttributes?.Where(x => Filter.Contains(x.Key) == false).ToDictionary(x => x.Key, x => x.Value);

	private void ToggleMenu()
	{
		IsActive = !IsActive;
	}
}
