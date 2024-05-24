using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;

namespace easy_blazor_bulma;

/// <summary>
/// Simplifies usage of the <see cref="InputRadioGroup{TValue}"/> and <see cref="InputRadio{TValue}"/> components.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <remarks>
/// There is 1 additional attribute that can be used: item-class. It will apply CSS classes to the resulting element as per its name.
/// </remarks>
public partial class InputRadioGroupObject<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : InputBase<TValue>
{
	/// <summary>
	/// The options to generate radio buttons for. Keys are display text, values are any bindable object.
	/// </summary>
	[Parameter]
	public Dictionary<string, TValue?> Options { get; set; } = default!;

	private readonly string[] Filter = new[] { "class", "item-class" };

	private readonly string PropertyName = Guid.NewGuid().ToString();
	private string CurrentValueDisplay = string.Empty;

	private string MainCssClass => CssClass;

	private string ItemCssClass => string.Join(' ', "is-checkradio is-primary", AdditionalAttributes.GetClass("item-class"));

	/// <inheritdoc/>
	protected override void OnInitialized()
	{
		var match = Options.Select(x => new { x.Key, x.Value }).FirstOrDefault(x => EqualityComparer<TValue>.Default.Equals(x.Value, Value));

		if (match != null)
			CurrentValueDisplay = match.Key;
	}

	/// <inheritdoc/>
	protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	protected override string FormatValueAsString(TValue? value) => value switch
	{
		TValue currentValue => currentValue.ToString() ?? string.Empty,
		_ => string.Empty
	};

	private void OnCurrentChanged(TValue? current)
	{
		var match = Options.Select(x => new { x.Key, x.Value }).FirstOrDefault(x => EqualityComparer<TValue>.Default.Equals(x.Value, current));

		if (match != null)
		{
			CurrentValueDisplay = match.Key;
			CurrentValue = current;
		}
	}

	private string GetRadioOptionId(string display) => $"radio-InputRadioGroupObject-{PropertyName}-{display.Replace(' ', '-')}";
}
