using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// A colored message block, to emphasize part of your page.
/// </summary>
/// <remarks>
/// <see href="https://bulma.io/documentation/components/message/">Bulma Documentation</see>
/// </remarks>
public partial class Message : ComponentBase
{
	/// <summary>
	/// Displays text in a bold bar at the top of the message.
	/// </summary>
	[Parameter]
	public string? Title { get; set; }

	/// <summary>
	/// Displays a button to hide the message from the display.
	/// </summary>
	[Parameter]
	public bool ShowDeleteButton { get; set; } = true;

	/// <summary>
	/// Specifies whether the message should be hidden from display.
	/// </summary>
	[Parameter]
	public bool IsHidden { get; set; }

    /// <summary>
    /// Sets the color to use for the message text and background.
    /// </summary>
    [Parameter]
    public BulmaColors Color { get; set; }

    /// <summary>
    /// The content to display within the message.
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
			var css = "message";

			if (IsHidden)
				css += " is-hidden";

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

	private void Delete()
	{
		IsHidden = true;
		StateHasChanged();
	}
}
