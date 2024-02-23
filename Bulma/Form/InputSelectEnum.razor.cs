using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// Creates a select list with the values of the provided enum. Supported types must inherit <see cref="Enum"/>.
/// </summary>
/// <typeparam name="TEnum"></typeparam>
/// <remarks>
/// <see href="https://bulma.io/documentation/form/select/">Bulma Documentation</see>
/// </remarks>
public partial class InputSelectEnum<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TEnum> : InputBase<TEnum>
{
    /// <summary>
	/// An icon to display within the input.
	/// </summary>
	[Parameter]
    public string? Icon { get; set; } = "list";

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

    /// <inheritdoc cref="InputDateTimeOptions.UseAutomaticStatusColors"/>
    [Parameter]
    public bool UseAutomaticStatusColors { get; set; } = true;

    /// <summary>
    /// Specifies whether to hide the option assigned to 0.
    /// </summary>
    [Parameter]
    public bool HideZeroOption { get; set; }

    private readonly bool IsNullable;
    private readonly Type UnderlyingType;

    private string FullCssClass
    {
        get
        {
            var css = "select";

            if (DisplayStatus.HasFlag(InputStatus.BackgroundDanger))
                css += " is-danger";
            else if (DisplayStatus.HasFlag(InputStatus.BackgroundWarning))
                css += " is-warning";
            else if (DisplayStatus.HasFlag(InputStatus.BackgroundSuccess))
                css += " is-success";

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
        else if (Enum.TryParse(UnderlyingType, value, true, out object? parsed))
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
