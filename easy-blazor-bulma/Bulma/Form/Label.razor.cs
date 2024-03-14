using easy_core;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;

namespace easy_blazor_bulma;

/// <summary>
/// A label to display with form inputs.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <remarks>
/// There is 1 additional attribute that can be used: data-tooltip. It adds a hover tooltip to the element.
/// <see href="https://bulma.io/documentation/form/general/">Bulma Documentation</see>
/// </remarks>
public partial class Label<TValue> : ComponentBase
{
	/// <summary>
	/// An expression that returns the property to create a label for.
	/// </summary>
	[Parameter]
	public Expression<Func<TValue>>? For { get; set; }

	/// <summary>
	/// The text to display in the label.
	/// </summary>
	/// <remarks>
	/// Display text can be added either by setting explicity or automatically if the for value has a <see cref="DisplayAttribute.GetName"/>.
	/// </remarks>
	[Parameter]
	public string? DisplayText { get; set; }

	/// <summary>
	/// Sets the display mode for a tooltip when present.
	/// </summary>
	/// <remarks>
	/// Tooltips can be added either by using the data-tooltip attribute or if the bound value has a <see cref="DisplayAttribute.GetDescription"/>.
	/// </remarks>
	[Parameter]
	public TooltipOptions TooltipMode { get; set; } = TooltipOptions.Default;

	/// <summary>
	/// Any additional attributes applied directly to the component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object>? AdditionalAttributes { get; set; }

	private string? Tooltip;

	private string FullCssClass => string.Join(' ', "label", CssClass);

	private string TooltipCssClass
	{
		get
		{
			var css = "";

			if (string.IsNullOrWhiteSpace(Tooltip) == false)
			{
				if (TooltipMode.HasFlag(TooltipOptions.Top))
					css += " has-tooltip-top";
				else if (TooltipMode.HasFlag(TooltipOptions.Bottom))
					css += " has-tooltip-bottom";
				else if (TooltipMode.HasFlag(TooltipOptions.Left))
					css += " has-tooltip-left";
				else if (TooltipMode.HasFlag(TooltipOptions.Right))
					css += " has-tooltip-right";

				if (TooltipMode.HasFlag(TooltipOptions.HasArrow))
					css += " has-tooltip-arrow";

				if (TooltipMode.HasFlag(TooltipOptions.AlwaysActive))
					css += " has-tooltip-active";

				if (TooltipMode.HasFlag(TooltipOptions.Multiline))
					css += " has-tooltip-multiline";
			}

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

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		DisplayAttribute? attribute = null;

		if (For != null)
			attribute = For.GetPropertyAttribute<TValue, DisplayAttribute>();

		if (string.IsNullOrWhiteSpace(DisplayText))
			DisplayText = attribute?.GetName();

		if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("data-tooltip", out var tooltip) && string.IsNullOrWhiteSpace(Convert.ToString(tooltip, CultureInfo.InvariantCulture)) == false)
			Tooltip = tooltip.ToString();
		else
			Tooltip = attribute?.GetDescription();
	}
}
