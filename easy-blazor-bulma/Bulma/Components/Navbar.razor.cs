﻿using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// A navigation menu to use at the top of the screen.
/// </summary>
/// <remarks>
/// <see href="https://bulma.io/documentation/components/navbar/">Bulma Documentation</see>
/// </remarks>
public partial class Navbar : ComponentBase
{
	/// <summary>
	/// The name to display in the top left of the navbar.
	/// </summary>
	[Parameter]
	public string? DisplayText { get; set; }

	/// <summary>
	/// The URL of an image to display in the top left of the navbar.
	/// </summary>
	[Parameter]
	public string? DisplayImageUrl { get; set; }

	/// <summary>
	/// Specifies whether to display the burger icon for mobile devices.
	/// </summary>
	[Parameter]
	public bool UseBurger { get; set; } = true;

	/// <summary>
	/// The content to display on the left side of the navbar.
	/// </summary>
	[Parameter]
	public RenderFragment? NavbarStart { get; set; }

	/// <summary>
	/// The content to display on the right side of the navbar.
	/// </summary>
	[Parameter]
	public RenderFragment? NavbarEnd { get; set; }

	/// <summary>
	/// Fallback for content to display within the navbar. Applies when neither <see cref="NavbarStart"/> nor <see cref="NavbarEnd"/> are provided.
	/// </summary>
	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	/// <summary>
	/// Any additional attributes applied directly to the component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object>? AdditionalAttributes { get; set; }

	private bool IsActive;
	private string? Id;
	private string Href = "";

	private string FullCssClass => string.Join(' ', "navbar", CssClass);

	private string BurgerCssClass
	{
		get
		{
			var css = "navbar-burger";

			if (IsActive)
				css += " is-active";

			return css;
		}
	}

	private string MenuCssClass
	{
		get
		{
			var css = "navbar-menu is-unselectable";

			if (IsActive)
				css += " is-active";

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

	private readonly string[] Filter = new[] { "class", "id", "role", "aria-label", "href" };
	private IReadOnlyDictionary<string, object>? FilteredAttributes => AdditionalAttributes?.Where(x => Filter.Contains(x.Key) == false).ToDictionary(x => x.Key, x => x.Value);

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		if (string.IsNullOrWhiteSpace(Id))
		{
			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("id", out var id) && string.IsNullOrWhiteSpace(Convert.ToString(id, CultureInfo.InvariantCulture)) == false)
				Id = id.ToString();
			else
				Id = Guid.NewGuid().ToString();
		}

		if (string.IsNullOrWhiteSpace(Href))
		{
			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("href", out var href) && string.IsNullOrWhiteSpace(Convert.ToString(href, CultureInfo.InvariantCulture)) == false)
				Href = href.ToString() ?? string.Empty;
		}
	}

	private void ToggleMenu()
	{
		IsActive = !IsActive;
	}
}
