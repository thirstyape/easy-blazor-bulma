namespace easy_blazor_bulma;

/// <summary>
/// A listing of options that will apply various color options to the input and its icon.
/// </summary>
[Flags]
public enum InputStatus
{
    /// <summary>
    /// No CSS color options will be applied
    /// </summary>
    None = 0b_00000000_00000000_00000000_00000000,

    /// <summary>
    /// The success CSS color will be applied to the input background
    /// </summary>
    BackgroundSuccess = 0b_00000000_00000000_00000000_00000001,

    /// <summary>
    /// The warning CSS color will be applied to the input background
    /// </summary>
    BackgroundWarning = 0b_00000000_00000000_00000000_00000010,

    /// <summary>
    /// The danger CSS color will be applied to the input background
    /// </summary>
    BackgroundDanger = 0b_00000000_00000000_00000000_00000100,

    /// <summary>
    /// The success CSS color will be applied to the input icon
    /// </summary>
    IconSuccess = 0b_00000000_00000000_00000000_00001000,

    /// <summary>
    /// The warning CSS color will be applied to the input icon
    /// </summary>
    IconWarning = 0b_00000000_00000000_00000000_00010000,

    /// <summary>
    /// The danger CSS color will be applied to the input icon
    /// </summary>
    IconDanger = 0b_00000000_00000000_00000000_00100000
}
