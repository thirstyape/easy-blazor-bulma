﻿using Microsoft.AspNetCore.Components;
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
public partial class InputFlaggedEnum<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TEnum> : InputBase<TEnum>
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

        if (typeof(Enum).IsAssignableFrom(UnderlyingType) == false)
            throw new InvalidOperationException($"Unsupported type param '{UnderlyingType.Name}'. Must inherit {nameof(Enum)}.");
        else if (Enum.GetUnderlyingType(typeof(TEnum)) == typeof(ulong))
            throw new InvalidOperationException($"Unsupported type param '{UnderlyingType.Name}'. Does not support enums based on ulong.");
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
        TEnum enumValue => enumValue.ToString() ?? string.Empty,
        _ => string.Empty
    };

    private void OnValueChanged(TEnum e)
    {
        var current = (Enum)Enum.Parse(UnderlyingType, (Value ?? default)?.ToString() ?? string.Empty);
        var flag = (Enum)Enum.Parse(UnderlyingType, e?.ToString() ?? string.Empty);

        if (Value == null)
            current = flag;
        else if (current.HasFlag(flag))
            current = (Enum)Enum.ToObject(typeof(TEnum), Convert.ToInt64(Value) & ~Convert.ToInt64(e));
        else
            current = (Enum)Enum.ToObject(typeof(TEnum), Convert.ToInt64(Value) | Convert.ToInt64(e));

        CurrentValueAsString = current?.ToString();
    }

    private bool IsFlagChecked(TEnum value)
    {
        var current = (Enum)Enum.Parse(UnderlyingType, (Value ?? default)?.ToString() ?? string.Empty);
        var flag = (Enum)Enum.Parse(UnderlyingType, value?.ToString() ?? string.Empty);

        if (Value == null)
            return false;
        else
            return current.HasFlag(flag);
    }

    private string GetEnumSwitchId(TEnum value) => $"switch-InputFlaggedEnum-{PropertyName}-{value}";
}
