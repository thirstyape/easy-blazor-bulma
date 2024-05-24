using Microsoft.AspNetCore.Components;

namespace easy_blazor_bulma;

/// <summary>
/// Use to create a single item within a <see cref="BreadCrumb"/> component.
/// </summary>
public partial class BreadCrumbItem : ComponentBase, IDisposable
{
    /// <summary>
    /// The text to show for this item.
    /// </summary>
    [Parameter]
    public string DisplayText { get; set; } = string.Empty;

    /// <summary>
    /// The URL to redirect to when clicking this item.
    /// </summary>
    [Parameter]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// An icon to display to the left of this item.
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }

    [CascadingParameter]
    private BreadCrumb Parent { get; set; } = default!;

	internal int Index;
	internal bool IsLast;

    /// <inheritdoc/>
	protected override void OnInitialized()
    {
        Parent.AddChild(this);
    }

    /// <inheritdoc/>
	public void Dispose()
    {
        Parent.RemoveChild(this);
        GC.SuppressFinalize(this);
    }
}
