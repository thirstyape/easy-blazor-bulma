using easy_core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
		InputAutocompleteOptions.ClickPopout |
		InputAutocompleteOptions.PopoutBottom |
		InputAutocompleteOptions.PopoutLeft |
		InputAutocompleteOptions.UseAutomaticStatusColors |
		InputAutocompleteOptions.AutoSelectOnExit |
		InputAutocompleteOptions.AutoSelectOnInput |
		InputAutocompleteOptions.AutoSelectExact;

	/// <summary>
	/// Fires with the oninput event just before updating the value of the input.
	/// </summary>
	[Parameter]
	public Func<string?, Task>? OnItemsRequested { get; set; }

	[Inject]
	private IServiceProvider ServiceProvider { get; init; }

	private bool IsPopoutDisplayed;
	private TValue? HighlightedValue;
	private string? InputValue;

	private readonly bool IsNullable;
	private readonly Type UnderlyingType;
	private ElementReference? Element;
	private ILogger<InputAutocomplete<TValue>>? Logger;

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
			var css = "dropdown-menu p-0";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("dropdown-menu-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
		}
	}

	private readonly string[] Filter = new[] { "class", "dropdown-class", "dropdown-trigger-class", "dropdown-menu-class", "dropdown-item-class" };
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
		// Get services
		Logger = ServiceProvider.GetService<ILogger<InputAutocomplete<TValue>>>();

		// Validation
		if (Options.HasAnyFlag(InputAutocompleteOptions.ClickPopout | InputAutocompleteOptions.TypePopout | InputAutocompleteOptions.HoverPopout) == false)
			Logger?.LogWarning("Must set at least one of ClickPopout, TypePopout, or HoverPopout for InputAutocomplete to work correctly.");

		// Set required options
		if (Options.HasAnyFlag(InputAutocompleteOptions.AutoSelectOnExit | InputAutocompleteOptions.AutoSelectOnInput) == false && Options.HasAnyFlag(InputAutocompleteOptions.AutoSelectCurrent | InputAutocompleteOptions.AutoSelectExact | InputAutocompleteOptions.AutoSelectClosest))
			Options |= InputAutocompleteOptions.AutoSelectOnExit;

		// Unset invalid options
		if (Options.HasFlag(InputAutocompleteOptions.PopoutLeft | InputAutocompleteOptions.PopoutRight))
			Options &= ~InputAutocompleteOptions.PopoutRight;

		if (Options.HasFlag(InputAutocompleteOptions.PopoutTop | InputAutocompleteOptions.PopoutBottom))
			Options &= ~InputAutocompleteOptions.PopoutTop;

		if (Options.HasFlag(InputAutocompleteOptions.AutoSelectCurrent | InputAutocompleteOptions.AutoSelectExact))
			Options &= ~InputAutocompleteOptions.AutoSelectCurrent;

		if (Options.HasFlag(InputAutocompleteOptions.AutoSelectCurrent | InputAutocompleteOptions.AutoSelectClosest))
			Options &= ~InputAutocompleteOptions.AutoSelectClosest;

		if (Options.HasFlag(InputAutocompleteOptions.AutoSelectExact | InputAutocompleteOptions.AutoSelectClosest))
			Options &= ~InputAutocompleteOptions.AutoSelectClosest;

		// Set starting values
		HighlightedValue = CurrentValue;
		InputValue = CurrentValueAsString;
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

		var match = Options.HasAnyFlag(InputAutocompleteOptions.AutoSelectExact | InputAutocompleteOptions.AutoSelectClosest) ? GetMatch(value) : GetMatch(value, InputAutocompleteOptions.AutoSelectExact);

		if (match.success)
		{
			result = match.match!;
			HighlightedValue = result;

			if (Options.HasFlag(InputAutocompleteOptions.UseAutomaticStatusColors))
				DisplayStatus |= InputStatus.BackgroundSuccess;

			validationErrorMessage = null;
			return true;
		}
		else
		{
			result = default;
			HighlightedValue = default;

			if (Options.HasFlag(InputAutocompleteOptions.UseAutomaticStatusColors))
				DisplayStatus |= InputStatus.BackgroundDanger;

			validationErrorMessage = string.Format(CultureInfo.InvariantCulture, "No match could be found in the {0} field.", DisplayName ?? FieldIdentifier.FieldName);
			return false;
		}
	}

	/// <inheritdoc />
	protected override string FormatValueAsString(TValue? value) => value switch
	{
		TValue selectedValue => DisplayValue(selectedValue),
		_ => string.Empty
	};

	private (bool success, TValue? match) GetMatch(string? value, InputAutocompleteOptions? matchType = null)
	{
		if (matchType == null)
		{
			if (Options.HasFlag(InputAutocompleteOptions.AutoSelectCurrent))
				matchType = InputAutocompleteOptions.AutoSelectCurrent;
			else if (Options.HasFlag(InputAutocompleteOptions.AutoSelectExact))
				matchType = InputAutocompleteOptions.AutoSelectExact;
			else if (Options.HasFlag(InputAutocompleteOptions.AutoSelectClosest))
				matchType = InputAutocompleteOptions.AutoSelectClosest;
		}

		if (matchType == InputAutocompleteOptions.AutoSelectCurrent && HighlightedValue != null)
		{
			return (true, HighlightedValue);
		}
		else if (matchType == InputAutocompleteOptions.AutoSelectExact)
		{
			var match = Items.FirstOrDefault(x => string.Equals(DisplayValue(x), value, StringComparison.OrdinalIgnoreCase));
			return (match != null || (IsNullable && string.IsNullOrWhiteSpace(value)), match);
		}
		else if (matchType == InputAutocompleteOptions.AutoSelectClosest)
		{
			return (true, Items.OrderBy(x => string.Compare(DisplayValue(x), value, StringComparison.OrdinalIgnoreCase)).FirstOrDefault());
		}
		else
		{
			return (false, default);
		}
	}

	private void OnFocus(FocusEventArgs args)
	{
		if (Options.HasFlag(InputAutocompleteOptions.ClickPopout) && Items.Any())
			IsPopoutDisplayed = true;
	}

	private void OnBlur(FocusEventArgs args)
	{
		IsPopoutDisplayed = false;

		if (Options.HasFlag(InputAutocompleteOptions.AutoSelectOnExit))
		{
			var match = GetMatch(InputValue);
			OnItemSelected(match.match, false, match.success);
		}
	}

	private void OnClick(MouseEventArgs args)
	{
		if (Options.HasFlag(InputAutocompleteOptions.ClickPopout) && Items.Any())
			IsPopoutDisplayed = true;
	}

	private async Task OnInput(ChangeEventArgs args)
	{
		if (Options.HasFlag(InputAutocompleteOptions.TypePopout) && Items.Any())
			IsPopoutDisplayed = true;

		var changed = args.Value?.ToString();

		if (OnItemsRequested != null)
			await OnItemsRequested.Invoke(changed);

		InputValue = changed;

		if (Options.HasFlag(InputAutocompleteOptions.AutoSelectOnInput))
			CurrentValueAsString = changed;
	}

	private void OnKeyUp(KeyboardEventArgs args)
	{
		if (args.Code == "Enter")
		{
			var match = GetMatch(InputValue, InputAutocompleteOptions.AutoSelectCurrent);
			OnItemSelected(match.match, success: match.success);
		}

		if (args.Code == "Enter" || args.Code == "Escape")
			IsPopoutDisplayed = false;
	}

	private void OnKeyDown(KeyboardEventArgs args)
	{
		OnKeyDownPreventDefault = args.Code == "ArrowDown" || args.Code == "ArrowUp" || args.Code == "Escape";

		if (Options.HasFlag(InputAutocompleteOptions.TypePopout) && Items.Any() && (args.Code == "ArrowDown" || args.Code == "ArrowUp"))
			IsPopoutDisplayed = true;

		if (IsPopoutDisplayed && args.Code == "ArrowDown")
			HighlightNext();
		else if (IsPopoutDisplayed && args.Code == "ArrowUp")
			HighlightPrevious();
	}

	private void HighlightNext()
	{
		if (HighlightedValue == null)
		{
			HighlightedValue = Items.FirstOrDefault();
			return;
		}

		TValue? next = default;
		TValue? first = default;
		bool takeNext = false;

		foreach (var item in Items)
		{
			first ??= item;

			if (takeNext)
			{
				next = item;
				break;
			}

			if (EqualityComparer<TValue>.Default.Equals(HighlightedValue, item))
				takeNext = true;
		}

		next ??= first;
		HighlightedValue = next;
	}

	private void HighlightPrevious()
	{
		if (HighlightedValue == null)
		{
			HighlightedValue = Items.LastOrDefault();
			return;
		}

		TValue? previous = default;

		foreach (var item in Items)
		{
			if (previous != null && EqualityComparer<TValue>.Default.Equals(HighlightedValue, item))
				break;

			previous = item;
		}

		HighlightedValue = previous;
	}

	private void OnItemSelected(TValue? value, bool close = true, bool success = true)
	{
		CurrentValueAsString = value != null ? DisplayValue(value) : string.Empty;
		InputValue = CurrentValueAsString;

		if (close)
			IsPopoutDisplayed = false;

		if (Options.HasFlag(InputAutocompleteOptions.UseAutomaticStatusColors))
		{
			ResetStatus();
			DisplayStatus |= success ? InputStatus.BackgroundSuccess : InputStatus.BackgroundDanger;
		}
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

		if (HighlightedValue != null && EqualityComparer<TValue>.Default.Equals(HighlightedValue, item))
			css += " has-background-default";

		if (CurrentValue != null && EqualityComparer<TValue>.Default.Equals(CurrentValue, item))
			css += " has-text-success";

		if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("dropdown-item-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
			css += $" {additional}";

		return css;
	}
}
