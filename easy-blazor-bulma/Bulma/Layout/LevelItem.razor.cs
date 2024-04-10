using Microsoft.AspNetCore.Components;

namespace easy_blazor_bulma;

/// <summary>
/// Creates an item within a level.
/// </summary>
/// <remarks>
/// <see href="https://bulma.io/documentation/layout/level/">Bulma Documentation</see>
/// </remarks>
public partial class LevelItem : ComponentBase
{
	/// <summary>
	/// The text to display above the title of the level item.
	/// </summary>
	[Parameter]
	public string? Heading { get; set; }

	/// <summary>
	/// The text to display as the main part of the level item.
	/// </summary>
	[Parameter]
	public string? Title { get; set; }

	/// <summary>
	/// The content to display within the level item.
	/// </summary>
	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	/// <summary>
	/// Any additional attributes applied directly to the component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object>? AdditionalAttributes { get; set; }

	private readonly string[] Filter = new[] { "class" };

	private string MainCssClass => string.Join(' ', "level-item", AdditionalAttributes.GetClass("class"));
}
