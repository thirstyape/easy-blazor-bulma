using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// Creates a checkbox list with the values of the provided flagged enum. Supported types must inherit <see cref="Enum"/>, but not <see cref="ulong"/>.
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

	private readonly string[] Filter = new[] { "class" };

	private readonly bool IsNullable;
    private readonly Type UnderlyingType;
    private readonly string PropertyName = Guid.NewGuid().ToString();

    private string MainCssClass
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
        else if (Enum.GetUnderlyingType(UnderlyingType) == typeof(ulong))
            throw new InvalidOperationException($"Unsupported type param '{UnderlyingType.Name}'. Does not support enums based on ulong.");
    }

    /// <inheritdoc/>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TEnum result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (IsNullable == false && string.IsNullOrWhiteSpace(value))
        {
            result = default!;

            validationErrorMessage = null;
            return true;
        }
        else if (Enum.TryParse(UnderlyingType, value, true, out object? parsed))
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

    private void OnValueChanged(TEnum flag)
    {
        var current = CurrentValue != null ? Convert.ToInt64(CurrentValue) : 0L;
        var update = Convert.ToInt64(flag);

        if ((current & update) != 0)
            current &= ~update;
        else
            current |= update;

        CurrentValueAsString = Enum.Parse(UnderlyingType, current.ToString()).ToString();
    }

    private bool IsFlagChecked(TEnum flag)
    {
        return CurrentValue != null && (Convert.ToInt64(CurrentValue) & Convert.ToInt64(flag)) != 0;
    }

    private string GetEnumSwitchId(TEnum value) => $"switch-InputFlaggedEnum-{PropertyName}-{value}";
}
