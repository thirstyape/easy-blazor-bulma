using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// Use to create a single step within a <see cref="Steps"/> component.
/// </summary>
/// <remarks>
/// There are 2 additional attributes that can be used: marker-class and content-class. Each of which apply CSS classes to the resulting elements as per their names.
/// <see href="https://octoshrimpy.github.io/bulma-o-steps/">GitHub Documentation</see>
/// </remarks>
public partial class Step : ComponentBase, IAsyncDisposable
{
    /// <summary>
	/// The name to display this step as in the list.
	/// </summary>
	[Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// An icon to display beside the name of the step in the list.
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    /// Specifies the color to assign to the step marker.
    /// </summary>
    [Parameter]
    public BulmaColors MarkerColor { get; set; }

    [CascadingParameter]
    private Steps Parent { get; set; }

    /// <summary>
    /// Any additional attributes applied directly to the component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    internal readonly string[] Filter = new[] { "class", "marker-class", "content-class" };
    internal bool IsActive => Name != null && Parent.Active == Name;
    internal int Index;

    /// <inheritdoc/>
	protected async override Task OnInitializedAsync()
    {
        await Parent.AddChild(this);
    }

    /// <inheritdoc/>
	public async ValueTask DisposeAsync()
	{
		await Parent.RemoveChild(this);
		GC.SuppressFinalize(this);
	}
}
