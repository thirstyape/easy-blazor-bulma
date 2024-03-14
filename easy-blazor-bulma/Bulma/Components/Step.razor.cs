using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// Use to create a single step within a <see cref="Steps"/> component.
/// </summary>
/// <remarks>
/// <see href="https://octoshrimpy.github.io/bulma-o-steps/">GitHub Documentation</see>
/// </remarks>
public partial class Step : ComponentBase, IDisposable
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

    internal bool IsActive => Name != null && Parent.Active == Name;
    internal int Index;

    internal string? CssClass
    {
        get
        {
            if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("class", out var css) && string.IsNullOrWhiteSpace(Convert.ToString(css, CultureInfo.InvariantCulture)) == false)
                return css.ToString();

            return "is-size-4";
        }
    }

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
