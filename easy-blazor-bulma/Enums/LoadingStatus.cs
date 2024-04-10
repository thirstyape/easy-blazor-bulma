namespace easy_blazor_bulma;

/// <summary>
/// Used to indicate the current status of the component that is loading.
/// </summary>
[Flags]
public enum LoadingStatus
{
    /// <summary>
    /// Loading has not begun.
    /// </summary>
    NotStarted = 0b_00000000_00000000_00000000_00000000,

    /// <summary>
    /// Loading is currently occurring.
    /// </summary>
    InProgress = 0b_00000000_00000000_00000000_00000001,

    /// <summary>
    /// Loading has finished.
    /// </summary>
    Complete = 0b_00000000_00000000_00000000_00000010,

    /// <summary>
    /// Loading performed as expected.
    /// </summary>
    Successful = 0b_00000000_00000000_00000000_00000100,

    /// <summary>
    /// Loading encountered an error.
    /// </summary>
    Failed = 0b_00000000_00000000_00000000_00001000
}
