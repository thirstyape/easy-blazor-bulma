using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEnum"></typeparam>
/// <remarks>
/// 
/// </remarks>
public partial class InputSelectEnum<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TEnum> : InputBase<TEnum>
{
    /// <summary>
    /// Specifies the text to display for the null option.
    /// </summary>
    [Parameter]
    public string NullText { get; set; } = "Null";

    /// <summary>
    /// Applies styles to the input.
    /// </summary>
    [Parameter]
	public InputStatus DisplayStatus { get; set; }

    /// <summary>
    /// Specifies whether to hide the option assigned to 0.
    /// </summary>
    [Parameter]
    public bool HideZeroOption { get; set; }

    /// <inheritdoc cref="InputDateTimeOptions.UseAutomaticStatusColors"/>
    [Parameter]
	public bool UseAutomaticStatusColors { get; set; } = true;

	private readonly bool IsNullable;
	private readonly Type UnderlyingType;

	private string FullCssClass
	{
		get
		{
			var css = "input";

			return string.Join(' ', css, CssClass);
		}
	}

	public InputSelectEnum()
	{
		var nullable = Nullable.GetUnderlyingType(typeof(TEnum));

		UnderlyingType = nullable ?? typeof(TEnum);
		IsNullable = nullable != null;

		if (typeof(Enum).IsAssignableFrom(UnderlyingType) == false)
            throw new InvalidOperationException($"Unsupported type param '{UnderlyingType.Name}'. Must inherit {nameof(Enum)}.");
    }

	/// <inheritdoc/>
	protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TEnum result, [NotNullWhen(false)] out string? validationErrorMessage)
	{
		if (IsNullable == false && string.IsNullOrWhiteSpace(value))
			value = "0";

		if (UseAutomaticStatusColors)
			ResetStatus();

		if (Enum.TryParse(UnderlyingType, value, true, out object? parsed))
		{
			result = (TEnum)parsed!;

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

			validationErrorMessage = string.Format(CultureInfo.InvariantCulture, "The {0} field could not be parsed.", DisplayName ?? FieldIdentifier.FieldName);
			return false;
		}
	}

	/// <inheritdoc />
	protected override string FormatValueAsString(TEnum? value) => value switch
	{
		TEnum enumValue => enumValue.ToString() ?? string.Empty,
		_ => string.Empty
	};

	private void OnSelectionChanged(ChangeEventArgs args)
	{
		CurrentValueAsString = args.Value?.ToString();
	}

	private void ResetStatus()
	{
		DisplayStatus &= ~InputStatus.BackgroundDanger;
		DisplayStatus &= ~InputStatus.BackgroundWarning;
		DisplayStatus &= ~InputStatus.BackgroundSuccess;
	}
}
