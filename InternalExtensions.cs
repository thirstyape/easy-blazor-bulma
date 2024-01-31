using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace easy_blazor_bulma;

/// <summary>
/// Extension methods to assist with displaying values.
/// </summary>
internal static class InternalExtensions
{
    /// <summary>
    /// Returns the matching attribute from the specified value if found.
    /// </summary>
    /// <typeparam name="TModel">The datatype of the property to check.</typeparam>
    /// <typeparam name="TAttribute">The datatype of the attribute to return.</typeparam>
    /// <param name="value">The value to check.</param>
    internal static TAttribute? GetValueAttribute<TModel, TAttribute>(this TModel value) where TAttribute : Attribute
    {
        var memberName = value?.ToString();

        if (value == null || string.IsNullOrWhiteSpace(memberName))
            return null;

        var attribute = value.GetType()
            .GetMember(memberName)
            .First()
            .GetCustomAttribute<TAttribute>();

        return attribute;
    }

    /// <summary>
    /// Returns the <see cref="DisplayAttribute.Name"/> value of the selected flags in a CSV string.
    /// </summary>
    /// <typeparam name="TEnum">The type of the property.</typeparam>
    /// <param name="value">The value to find the name of.</param>
    /// <param name="ignoreZero">Specifies whether to ignore the flag with a zero value.</param>
    internal static string GetFlaggedEnumDisplay<TEnum>(this TEnum value, bool ignoreZero = true) where TEnum : Enum
    {
        var display = "";

        foreach (Enum flag in Enum.GetValues(value.GetType()))
        {
            if (ignoreZero && (int)(object)flag == 0)
                continue;
            else if (value.HasFlag(flag) == false)
                continue;

            var current = ((TEnum)flag).GetValueAttribute<TEnum, DisplayAttribute>()?.GetName();

            display += (string.IsNullOrWhiteSpace(current) == false ? current : flag.ToString()) + ' ';
        }

        return display.TrimEnd();
    }
}
