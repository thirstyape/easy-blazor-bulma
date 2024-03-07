using easy_core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// An input component for toggling boolean values. Supported types are <see cref="bool"/>.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <remarks>
/// There are 3 additional attributes that can be used: div-class, label-class, and data-tooltip. The first two apply CSS classes to the resulting elements as per their names. The last adds a hover tooltip to the element.
/// </remarks>
public partial class InputSwitch<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : InputBase<TValue>
{
    /// <summary>
    /// Text to display to the right of the switch. Automatically takes <see cref="DisplayAttribute.GetName"/> when available.
    /// </summary>
    [Parameter]
	public string? Label { get; set; }

    /// <summary>
    /// Specifies whether to display the resulting switch in a box.
    /// </summary>
    [Parameter]
	public bool IsBoxed { get; set; } = true;

	/// <summary>
	/// Sets the display mode for a tooltip when present.
	/// </summary>
	/// <remarks>
	/// Tooltips can be added either by using the data-tooltip attribute or if the bound value has a <see cref="DisplayAttribute.GetDescription"/>.
	/// </remarks>
	[Parameter]
	public TooltipOptions TooltipMode { get; set; } = TooltipOptions.Default;

    private readonly bool IsNullable;
	private readonly Type UnderlyingType;

    private string? Id;
    private string? Tooltip;

    private string FullCssClass
	{
		get
		{
			return string.Join(' ', "switch", CssClass);
		}
	}

	private string LabelCssClass
	{
		get
		{
			var css = "no-select";

			if (string.IsNullOrWhiteSpace(Tooltip) == false)
				css += " ";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("label-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
		}
	}

	private string DivCssClass
	{
		get
		{
			var css = "checkbox";

			if (IsBoxed)
				css += " box";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("div-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
		}
	}

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

    private readonly string[] Filter = new string[] { "class", "id", "data-tooltip" };
    private IReadOnlyDictionary<string, object>? FilteredAttributes => AdditionalAttributes?.Where(x => Filter.Contains(x.Key) == false).ToDictionary(x => x.Key, x => x.Value);

    public InputSwitch()
	{
        var nullable = Nullable.GetUnderlyingType(typeof(TValue));

        UnderlyingType = nullable ?? typeof(TValue);
        IsNullable = nullable != null;

        if (UnderlyingType != typeof(bool))
            throw new InvalidOperationException($"Unsupported type param '{UnderlyingType.Name}'. Must be of type bool.");
    }

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

		if (ValueExpression != null)
		{
			var attribute = ValueExpression.GetPropertyAttribute<TValue, DisplayAttribute>();

			if (string.IsNullOrWhiteSpace(Label))
				Label = attribute?.GetName();

            if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("data-tooltip", out var tooltip) && string.IsNullOrWhiteSpace(Convert.ToString(tooltip, CultureInfo.InvariantCulture)) == false)
                Tooltip = tooltip.ToString();
            else
                Tooltip = attribute?.GetDescription();
        }
	}

    /// <inheritdoc/>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	protected override string FormatValueAsString(TValue? value) => value switch
	{
		bool boolValue => boolValue.ToString(),
		_ => string.Empty
	};
}
