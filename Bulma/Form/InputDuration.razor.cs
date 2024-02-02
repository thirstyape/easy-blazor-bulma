using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace easy_blazor_bulma;

/// <summary>
/// An input component for editing duration values. Supported types are <see cref="TimeSpan"/> and <see cref="TimeOnly"/>.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public partial class InputDuration<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : InputBase<TValue>
{
	/// <summary>
	/// The number of minutes or seconds to adjust by when the up or down arrows are clicked.
	/// </summary>
	[Parameter]
	[Range(1, 60)]
	public int Step { get; set; } = 5;

	/// <summary>
	/// An icon to display within the input.
	/// </summary>
	[Parameter]
	public string? Icon { get; set; }

	/// <summary>
	/// Applies styles to the input according to the selected options. Will automatically update 
	/// </summary>
	[Parameter]
	public InputStatus DisplayStatus { get; set; }

	/// <summary>
	/// The configuration options to apply to the component.
	/// </summary>
	[Parameter]
	public InputDurationOptions Options { get; set; } = InputDurationOptions.ClickPopout | InputDurationOptions.PopoutBottom | InputDurationOptions.PopoutLeft | InputDurationOptions.ShowResetButton | InputDurationOptions.UpdateOnPopoutChange | InputDurationOptions.UseAutomaticStatusColors;

	private TimeSpan PopoutValue;
	private bool IsPopoutDisplayed;

	private readonly bool IsNullable;
	private readonly Type UnderlyingType;

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

	private string TimePickerCssClass
	{
		get
		{
			var css = "datetimepicker";

			if (Options.HasFlag(InputDurationOptions.HoverPopout))
				css += " is-hoverable";
			else if (IsPopoutDisplayed && AdditionalAttributes?.Any(x => x.Key == "readonly" || (x.Key == "disabled" && (x.Value.ToString() == "disabled" || x.Value.ToString() == "true"))) == false)
				css += " is-active";

			if (Options.HasFlag(InputDurationOptions.PopoutBottom))
				css += " datetimepicker-below";
			else if (Options.HasFlag(InputDurationOptions.PopoutTop))
				css += " datetimepicker-above";

			if (Options.HasFlag(InputDurationOptions.PopoutLeft))
				css += " datetimepicker-left";
			else if (Options.HasFlag(InputDurationOptions.PopoutRight))
				css += " datetimepicker-right";

			return css;
		}
	}

	private string IconCssClass
	{
		get
		{
			var css = "material-icons icon is-left";

			if (FullCssClass.Contains("is-small"))
				css += " is-small";

			if (DisplayStatus.HasFlag(InputStatus.IconDanger))
				css += " has-text-danger";
			else if (DisplayStatus.HasFlag(InputStatus.IconWarning))
				css += " has-text-warning";
			else if (DisplayStatus.HasFlag(InputStatus.IconSuccess))
				css += " has-text-success";

			return css;
		}
	}

	public InputDuration()
	{
		var nullable = Nullable.GetUnderlyingType(typeof(TValue));

		UnderlyingType = nullable ?? typeof(TValue);
		IsNullable = nullable != null;

		if (UnderlyingType != typeof(TimeSpan) && UnderlyingType != typeof(TimeOnly))
			throw new InvalidOperationException($"Unsupported type param '{UnderlyingType.Name}'. Must be of type {nameof(TimeSpan)} or {nameof(TimeOnly)}.");
	}

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		if (IsNullable && Value == null)
			PopoutValue = TimeSpan.Zero;
		else if (UnderlyingType == typeof(TimeSpan))
			PopoutValue = (TimeSpan)Convert.ChangeType(Value!, typeof(TimeSpan));
		else
			PopoutValue = ((TimeOnly)(object)Value!).ToTimeSpan();
	}

	/// <inheritdoc/>
	protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	protected override string FormatValueAsString(TValue? value)
	{
		return "";
	}

	private async Task CheckKeyPress(KeyboardEventArgs args)
	{
		if (args.Key == "Escape" || args.Key == "Tab")
			await ClosePopup();
	}

	private void OpenPopup()
	{
		if (IsPopoutDisplayed || Options.HasFlag(InputDurationOptions.NoPopout))
			return;

		IsPopoutDisplayed = true;

		if (Options.HasFlag(InputDurationOptions.UseAutomaticStatusColors))
			ResetStatus();
	}

	private async Task ClosePopup(bool save = false, bool reset = false)
	{

	}

	private async Task OnChange(ChangeEventArgs args)
	{

	}

	private void UpdatePopupValue(TimeSpan adjustment)
	{

	}

	private void ResetStatus()
	{
		DisplayStatus &= ~InputStatus.BackgroundDanger;
		DisplayStatus &= ~InputStatus.BackgroundWarning;
		DisplayStatus &= ~InputStatus.BackgroundSuccess;
	}
}
