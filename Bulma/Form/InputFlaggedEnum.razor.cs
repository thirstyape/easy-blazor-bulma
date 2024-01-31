using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// Creates a checkbox list with the values of the provided flagged enum.
/// </summary>
/// <typeparam name="TEnum">The type of enum to use.</typeparam>
/// <remarks>
/// <see href="https://bulma.io/documentation/form/checkbox/">Bulma Documentation</see>
/// </remarks>
public partial class InputFlaggedEnum<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TEnum> : InputBase<TEnum> where TEnum : Enum
{
    /// <summary>
    /// Wraps each item in the .box CSS class when true.
    /// </summary>
    [Parameter]
    public bool IsBoxed { get; set; } = true;

    /// <summary>
    /// Specifies whether to hide the option assigned to 0.
    /// </summary>
    [Parameter]
    public bool HideZeroOption { get; set; } = true;

    private readonly bool IsNullable;
    private readonly Type UnderlyingType;
    private readonly string PropertyName = Guid.NewGuid().ToString();

    private string FullCssClass 
    { 
        get
        {
            var css = "checkbox";

            if (IsBoxed)
                css += " box";

            return string.Join(' ', css, CssClass);
        }
    }

    public InputFlaggedEnum()
    {
        var nullable = Nullable.GetUnderlyingType(typeof(TEnum));

        UnderlyingType = nullable ?? typeof(TEnum);
        IsNullable = nullable != null;
    }

    /// <inheritdoc/>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TEnum result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (IsNullable == false && string.IsNullOrWhiteSpace(value))
            value = "0";

        if (Enum.TryParse(UnderlyingType, value, true, out object? parsed))
        {
            result = (TEnum)parsed!;
            validationErrorMessage = null;

            return true;
        }
        else
        {
            result = default;
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, "The {0} field could not be parsed.", DisplayName ?? FieldIdentifier.FieldName);

            return false;
        }
    }

    /// <inheritdoc />
    protected override string FormatValueAsString(TEnum? value) => value switch
    {
        TEnum enumValue => enumValue.GetFlaggedEnumDisplay(),
        _ => string.Empty
    };

    private void OnValueChanged(TEnum e)
    {
        var value = Value ?? default;

        if (Value == null)
            value = e;
        else if (Value.HasFlag(e))
            value &= ~(dynamic)e;
        else
            value |= (dynamic)e;

        CurrentValueAsString = value?.ToString();
    }

    private string GetEnumSwitchId(TEnum value) => $"switch-InputFlaggedEnum-{PropertyName}-{value}";
}
