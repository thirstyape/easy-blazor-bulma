using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// An input component for entering numeric values. Supported types are <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="float"/>, <see cref="double"/>, and <see cref="decimal"/>.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <remarks>
/// There are 3 additional attributes that can be used: columns-class, column-class, and button-class. Each of which apply CSS classes to the resulting elements as per their names.
/// </remarks>
public partial class InputNumberPad<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : InputBase<TValue>
{
    /// <summary>
    /// An icon to display within the input.
    /// </summary>
    [Parameter]
    public string? Icon { get; set; } = "numbers";

    /// <summary>
    /// Specifies whether to show a text input along with the number pad.
    /// </summary>
    [Parameter]
	public bool DisplayInput { get; set; }

	/// <summary>
	/// Specifies whether to round the number selection buttons.
	/// </summary>
	[Parameter]
	public bool IsRounded { get; set; } = true;

	/// <summary>
	/// Specifies whether to apply a border to the number selection buttons.
	/// </summary>
	[Parameter]
	public bool IsBordered { get; set; } = true;

	/// <summary>
	/// Applies styles to the input according to the selected options. Requires <see cref="DisplayInput"/> to be on.
	/// </summary>
	[Parameter]
	public InputStatus DisplayStatus { get; set; }

    /// <inheritdoc cref="InputDateTimeOptions.UseAutomaticStatusColors"/>
    [Parameter]
    public bool UseAutomaticStatusColors { get; set; } = true;

    /// <summary>
    /// Sets the text to display on a button below the number pad. Requires a value in <see cref="OnCustomButtonClicked"/>.
    /// </summary>
    [Parameter]
	public string? CustomButtonText { get; set; }

	/// <summary>
	/// A function that will run when the button is clicked. This will create a button below the number pad that displays <see cref="CustomButtonText"/>.
	/// </summary>
	[Parameter]
	public Func<Task>? OnCustomButtonClicked { get; set; }

	private readonly bool IsNullable;
	private readonly Type UnderlyingType;
	private ElementReference? Element;

	private readonly bool SupportsDecimals;
	private bool OnKeyDownPreventDefault;

	private string DisplayValueAsString = string.Empty;

	private string FullCssClass
	{
		get
		{
			var css = "input mb-3";

			if (DisplayStatus.HasFlag(InputStatus.BackgroundDanger))
				css += " is-danger";
			else if (DisplayStatus.HasFlag(InputStatus.BackgroundWarning))
				css += " is-warning";
			else if (DisplayStatus.HasFlag(InputStatus.BackgroundSuccess))
				css += " is-success";

			return string.Join(' ', css, CssClass);
		}
	}

	private string ColumnsCssClass
	{
		get
		{
			var css = "columns mb-0";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("columns-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
		}
	}

	private string ColumnCssClass
	{
		get
		{
			var css = "column pb-0";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("column-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
		}
	}

	private string ButtonCssClass
	{
		get
		{
			var css = "button is-fullwidth mb-3";

			if (IsRounded)
				css += " is-rounded";

			if (IsBordered)
				css += " is-bordered";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("button-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
		}
	}

	private readonly string[] Filter = new string[] { "class", "columns-class", "column-class", "button-class" };
	private IReadOnlyDictionary<string, object>? FilteredAttributes => AdditionalAttributes?.Where(x => Filter.Contains(x.Key) == false).ToDictionary(x => x.Key, x => x.Value);

	public InputNumberPad()
	{
		var nullable = Nullable.GetUnderlyingType(typeof(TValue));

		UnderlyingType = nullable ?? typeof(TValue);
		IsNullable = nullable != null;

		if (UnderlyingType != typeof(short) && UnderlyingType != typeof(int) && UnderlyingType != typeof(long) && UnderlyingType != typeof(float) && UnderlyingType != typeof(double) && UnderlyingType != typeof(decimal))
			throw new InvalidOperationException($"Unsupported type param '{UnderlyingType.Name}'. Must be of type short, int, long, float, double, or decimal.");

		SupportsDecimals = UnderlyingType == typeof(float) || UnderlyingType == typeof(double) || UnderlyingType == typeof(decimal);
	}

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		if (string.IsNullOrWhiteSpace(CurrentValueAsString))
			DisplayValueAsString = string.Empty;
		else
			DisplayValueAsString = CurrentValueAsString;
	}

	/// <inheritdoc />
	protected async override Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			if (Element != null && AdditionalAttributes != null && AdditionalAttributes.TryGetValue("autofocus", out var _))
				await Element.Value.FocusAsync();
	}

	/// <inheritdoc />
	protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
	{
        if (UseAutomaticStatusColors)
            ResetStatus();

        if (IsNullable == false && string.IsNullOrWhiteSpace(value))
        {
            result = default!;

            if (UseAutomaticStatusColors)
                DisplayStatus |= InputStatus.BackgroundSuccess;

            validationErrorMessage = null;
            return true;
        }
        else if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result))
		{
            if (UseAutomaticStatusColors)
                DisplayStatus |= InputStatus.BackgroundSuccess;

            validationErrorMessage = null;
			return true;
		}
		else
		{
			result = default;

            if (UseAutomaticStatusColors)
                DisplayStatus |= InputStatus.BackgroundDanger;

            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, "The {0} field must be a char.", DisplayName ?? FieldIdentifier.FieldName);
			return false;
		}
	}

	/// <inheritdoc />
	protected override string FormatValueAsString(TValue? value) => value switch
	{
		short shortValue => shortValue.ToString("g"),
		int intValue => intValue.ToString("g"),
		long longValue => longValue.ToString("g"),
		float floatValue => floatValue.ToString("g"),
		double doubleValue => doubleValue.ToString("g"),
		decimal decimalValue => decimalValue.ToString("g"),
		_ => string.Empty
	};

	private void OnDigitClicked(int digit)
	{
		DisplayValueAsString += digit.ToString();

		if (DisplayValueAsString.Length > 1 && DisplayValueAsString[0] == '0' && DisplayValueAsString[1] != '.')
			DisplayValueAsString = DisplayValueAsString.TrimStart('0');

		CurrentValueAsString = DisplayValueAsString;
	}

	private void OnDecimalClicked()
	{
		if (DisplayValueAsString.Contains('.'))
			return;

		if (DisplayValueAsString.Length == 0)
			DisplayValueAsString = "0.";
		else
			DisplayValueAsString += '.';
	}

	private void OnBackspaceClicked()
	{
		if (DisplayValueAsString.Length == 0)
			return;

		if (IsNullable && DisplayValueAsString.Length == 1)
		{
			DisplayValueAsString = string.Empty;
			CurrentValueAsString = null;
		}
		else if (DisplayValueAsString.Length == 1)
		{
			DisplayValueAsString = "0";
			CurrentValueAsString = "0";
		}
		else
		{
			DisplayValueAsString = DisplayValueAsString[..^1];
			CurrentValueAsString = DisplayValueAsString;
		}
	}

	private void OnResetClicked()
	{
		if (IsNullable)
		{
			DisplayValueAsString = string.Empty;
			CurrentValueAsString = null;
		}
		else
		{
			DisplayValueAsString = "0";
			CurrentValueAsString = "0";
		}
	}

	private void OnKeyDown(KeyboardEventArgs args)
	{
		OnKeyDownPreventDefault = args.Code != "Escape" && args.Code != "Tab" && args.Code != "Enter" && args.Code != "NumpadEnter";

		if (NumberKeys.ContainsKey(args.Code))
			OnDigitClicked(NumberKeys[args.Code]);
		else if (args.Code == "Backspace")
			OnBackspaceClicked();
		else if (SupportsDecimals && (args.Code == "NumpadDecimal" || args.Code == "Period"))
			OnDecimalClicked();
	}

    private void ResetStatus()
    {
        DisplayStatus &= ~InputStatus.BackgroundDanger;
        DisplayStatus &= ~InputStatus.BackgroundWarning;
        DisplayStatus &= ~InputStatus.BackgroundSuccess;
    }

    private static readonly Dictionary<string, int> NumberKeys = new()
	{
		["Digit0"] = 0,
		["Digit1"] = 1,
		["Digit2"] = 2,
		["Digit3"] = 3,
		["Digit4"] = 4,
		["Digit5"] = 5,
		["Digit6"] = 6,
		["Digit7"] = 7,
		["Digit8"] = 8,
		["Digit9"] = 9,
		["Numpad0"] = 0,
		["Numpad1"] = 1,
		["Numpad2"] = 2,
		["Numpad3"] = 3,
		["Numpad4"] = 4,
		["Numpad5"] = 5,
		["Numpad6"] = 6,
		["Numpad7"] = 7,
		["Numpad8"] = 8,
		["Numpad9"] = 9
	};
}
