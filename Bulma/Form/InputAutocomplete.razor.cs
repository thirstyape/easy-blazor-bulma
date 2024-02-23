using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace easy_blazor_bulma;

/// <summary>
/// An input component for selecting a value from a list of options. Supported types are inherit class.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <remarks>
/// There are 4 additional attributes that can be used: dropdown-class, dropdown-trigger-class, dropdown-menu-class, and dropdown-item-class. Each of which apply CSS classes to the resulting elements as per their names.
/// <see href="https://bulma.io/documentation/components/dropdown/">Bulma Documentation</see>
/// </remarks>
public partial class InputAutocomplete<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : InputBase<TValue>
{
	/// <summary>
	/// The collection of items to search when typing in the input.
	/// </summary>
    [Parameter]
    public IEnumerable<TValue> Items { get; set; }

	/// <summary>
	/// Limits the number of items displayed in the drop-down list when set.
	/// </summary>
	[Parameter]
	public int? DisplayCount { get; set; }

	/// <summary>
	/// A function to return the values to display in the drop-down list.
	/// </summary>
    [Parameter] 
	public Func<TValue, string> DisplayValue { get; set; }

    /// <summary>
    /// An icon to display within the input.
    /// </summary>
    [Parameter]
	public string? Icon { get; set; } = "search";

	/// <summary>
	/// Applies styles to the input according to the selected options.
	/// </summary>
	[Parameter]
	public InputStatus DisplayStatus { get; set; }

	/// <summary>
	/// The configuration options to apply to the component.
	/// </summary>
	[Parameter]
    public InputAutocompleteOptions Options { get; set; } = 
		InputAutocompleteOptions.TypePopout |
		InputAutocompleteOptions.PopoutBottom |
		InputAutocompleteOptions.PopoutLeft |
		InputAutocompleteOptions.UseAutomaticStatusColors |
		InputAutocompleteOptions.AutoSelectMatch |
		InputAutocompleteOptions.AutoSelectExact;

    private bool IsPopoutDisplayed;

    private readonly bool IsNullable;
	private readonly Type UnderlyingType;
    private ElementReference? Element;

    private bool OnKeyDownPreventDefault;

	private bool Inactive => AdditionalAttributes != null && AdditionalAttributes.Any(x => x.Key == "readonly" || (x.Key == "disabled" && (x.Value.ToString() == "disabled" || x.Value.ToString() == "true")));

	private string FullCssClass
	{
		get
		{
			var css = "input";

			if (DisplayStatus.HasFlag(InputStatus.BackgroundDanger))
				css += " is-danger";
			else if (DisplayStatus.HasFlag(InputStatus.BackgroundWarning))
				css += " is-warning";
			else if (DisplayStatus.HasFlag(InputStatus.BackgroundSuccess))
				css += " is-success";

			return string.Join(' ', css, CssClass);
		}
	}

	private string DropDownCssClass
	{
		get
		{
			var css = "dropdown dropdown-block";

			if (IsPopoutDisplayed && Inactive == false)
				css += " is-active";

			if (Options.HasFlag(InputAutocompleteOptions.PopoutTop))
				css += " is-up";

			if (Options.HasFlag(InputAutocompleteOptions.PopoutRight))
				css += " is-right";

			if (Options.HasFlag(InputAutocompleteOptions.HoverPopout))
				css += " is-hoverable";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("dropdown-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
		}
	}

	private string DropDownTriggerCssClass
	{
		get
		{
			var css = "dropdown-trigger";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("dropdown-trigger-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
		}
	}

	private string DropDownMenuCssClass
	{
		get
		{
			var css = "dropdown-menu";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("dropdown-menu-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
		}
	}

	private readonly string[] Filter = new string[] { "class", "dropdown-class", "dropdown-trigger-class", "dropdown-menu-class", "dropdown-item-class" };
	private IReadOnlyDictionary<string, object>? FilteredAttributes => AdditionalAttributes?.Where(x => Filter.Contains(x.Key) == false).ToDictionary(x => x.Key, x => x.Value);

	public InputAutocomplete()
	{
		var nullable = Nullable.GetUnderlyingType(typeof(TValue));

		UnderlyingType = nullable ?? typeof(TValue);
		IsNullable = nullable != null;

		if (UnderlyingType.GetTypeInfo().IsClass == false)
			throw new InvalidOperationException($"Unsupported type param '{UnderlyingType.Name}'. Must be a class.");
	}

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		// Validation
		if (Options.HasFlag(InputAutocompleteOptions.ClickPopout | InputAutocompleteOptions.TypePopout | InputAutocompleteOptions.HoverPopout) == false)
			throw new ArgumentException("Must set at least one of ClickPopout, TypePopout, or HoverPopout.", nameof(Options));

		// Set required options
		if (Options.HasFlag(InputAutocompleteOptions.AutoSelectMatch) == false && Options.HasFlag(InputAutocompleteOptions.AutoSelectCurrent | InputAutocompleteOptions.AutoSelectExact | InputAutocompleteOptions.AutoSelectClosest))
			Options |= InputAutocompleteOptions.AutoSelectMatch;

		// Unset invalid options
		if (Options.HasFlag(InputAutocompleteOptions.PopoutLeft) && Options.HasFlag(InputAutocompleteOptions.PopoutRight))
			Options &= ~InputAutocompleteOptions.PopoutRight;

		if (Options.HasFlag(InputAutocompleteOptions.PopoutTop) && Options.HasFlag(InputAutocompleteOptions.PopoutBottom))
			Options &= ~InputAutocompleteOptions.PopoutTop;

		if (Options.HasFlag(InputAutocompleteOptions.AutoSelectCurrent) && Options.HasFlag(InputAutocompleteOptions.AutoSelectExact))
			Options &= ~InputAutocompleteOptions.AutoSelectCurrent;

		if (Options.HasFlag(InputAutocompleteOptions.AutoSelectCurrent) && Options.HasFlag(InputAutocompleteOptions.AutoSelectClosest))
			Options &= ~InputAutocompleteOptions.AutoSelectClosest;

		if (Options.HasFlag(InputAutocompleteOptions.AutoSelectExact) && Options.HasFlag(InputAutocompleteOptions.AutoSelectClosest))
			Options &= ~InputAutocompleteOptions.AutoSelectClosest;
	}

	/// <inheritdoc />
	protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            if (Element != null && AdditionalAttributes != null && AdditionalAttributes.TryGetValue("autofocus", out var _))
                await Element.Value.FocusAsync();
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
	{
		if (Options.HasFlag(InputAutocompleteOptions.UseAutomaticStatusColors))
			ResetStatus();

		if (IsNullable == false && string.IsNullOrWhiteSpace(value))
		{
			result = default!;

			if (Options.HasFlag(InputAutocompleteOptions.UseAutomaticStatusColors))
				DisplayStatus |= InputStatus.BackgroundSuccess;

			validationErrorMessage = null;
			return true;
		}

		throw new NotImplementedException();
	}

	/// <inheritdoc />
	protected override string FormatValueAsString(TValue? value) => value switch
	{
		TValue selectedValue => DisplayValue(selectedValue),
		_ => string.Empty
	};

	private void OnInput(ChangeEventArgs args)
	{

	}

	private void OpenPopout()
	{

	}

	private void CheckKeyPress(KeyboardEventArgs args)
	{

	}

	private void OnSelectionChanged(TValue value)
	{

	}

	private void ResetStatus()
	{
		DisplayStatus &= ~InputStatus.BackgroundDanger;
		DisplayStatus &= ~InputStatus.BackgroundWarning;
		DisplayStatus &= ~InputStatus.BackgroundSuccess;
	}

	private string GetDropDownItemCssClass(TValue item)
	{
		var css = "dropdown-item";

		if (Value != null && EqualityComparer<TValue>.Default.Equals(Value, item))
			css += " has-background-default";

		if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("dropdown-item-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
			css += $" {additional}";

		return css;
	}
}
