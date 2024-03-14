namespace easy_blazor_bulma;

/// <summary>
/// Specifies where the tooltip will be placed in relation to the element.
/// </summary>
[Flags]
public enum TooltipOptions
{
    /// <summary>
    /// The tooltip will use the standard options.
    /// </summary>
    Default = 0b_00000000_00000000_00000000_00000000,

    /// <summary>
    /// The tooltip will display above the element.
    /// </summary>
    Top = 0b_00000000_00000000_00000000_00000001,

    /// <summary>
    /// The tooltip will display below the element.
    /// </summary>
    Bottom = 0b_00000000_00000000_00000000_00000010,

    /// <summary>
    /// The tooltip will display to the left of the element.
    /// </summary>
    Left = 0b_00000000_00000000_00000000_00000100,

    /// <summary>
    /// The tooltip will display to the right of the element.
    /// </summary>
    Right = 0b_00000000_00000000_00000000_00001000,

    /// <summary>
    /// The tooltip will show an arrow directed towards the element.
    /// </summary>
    HasArrow = 0b_00000000_00000000_00000000_00010000,

    /// <summary>
    /// The tooltip will use multiple lines for display.
    /// </summary>
    Multiline = 0b_00000000_00000000_00000000_00100000,

    /// <summary>
    /// The tooltip will be displayed at all times.
    /// </summary>
    AlwaysActive = 0b_00000000_00000000_00000000_01000000
}
