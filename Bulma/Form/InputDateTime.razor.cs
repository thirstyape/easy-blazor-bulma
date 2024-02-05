using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace easy_blazor_bulma;

/// <summary>
/// An input component for editing date and time values. Supported types are <see cref="DateTime"/>, <see cref="DateOnly"/>, <see cref="TimeSpan"/>, and <see cref="TimeOnly"/>.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public partial class InputDateTime<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : InputBase<TValue>
{
	/// <summary>
	/// The number of hours to adjust by when the up or down arrows are clicked.
	/// </summary>
	[Parameter]
	[Range(1, 24)]
	public int StepHours { get; set; } = 1;

	/// <summary>
	/// The number of minutes to adjust by when the up or down arrows are clicked.
	/// </summary>
	[Parameter]
	[Range(1, 60)]
	public int StepMinutes { get; set; } = 5;

	/// <summary>
	/// The number of seconds to adjust by when the up or down arrows are clicked.
	/// </summary>
	[Parameter]
	[Range(1, 60)]
	public int StepSeconds { get; set; } = 15;

	/// <summary>
	/// Specifies the first day to display on the calendar view.
	/// </summary>
	[Parameter]
	public DayOfWeek StartOfWeek { get; set; } = DayOfWeek.Sunday;

	/// <summary>
	/// An icon to display within the input.
	/// </summary>
	[Parameter]
	public string? Icon { get; set; } = "calendar_month";

	/// <summary>
	/// Applies styles to the input according to the selected options.
	/// </summary>
	[Parameter]
	public InputStatus DisplayStatus { get; set; }

	/// <summary>
	/// The configuration options to apply to the component.
	/// </summary>
	[Parameter]
	public InputDateTimeOptions Options { get; set; } =
		InputDateTimeOptions.ClickPopout |
		InputDateTimeOptions.PopoutBottom |
		InputDateTimeOptions.PopoutLeft |
		InputDateTimeOptions.ShowNowButton |
		InputDateTimeOptions.ShowResetButton |
		InputDateTimeOptions.UpdateOnPopoutChange |
		InputDateTimeOptions.UseAutomaticStatusColors |
		InputDateTimeOptions.ShowHours |
		InputDateTimeOptions.ShowMinutes;

	private DateTime PopoutValue;
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

			if (Options.HasFlag(InputDateTimeOptions.HoverPopout))
				css += " is-hoverable";

			if (IsPopoutDisplayed && (AdditionalAttributes == null || AdditionalAttributes.Any(x => x.Key == "readonly" || (x.Key == "disabled" && (x.Value.ToString() == "disabled" || x.Value.ToString() == "true"))) == false))
				css += " is-active";

			if (Options.HasFlag(InputDateTimeOptions.PopoutBottom))
				css += " datetimepicker-below";
			else if (Options.HasFlag(InputDateTimeOptions.PopoutTop))
				css += " datetimepicker-above";

			if (Options.HasFlag(InputDateTimeOptions.PopoutLeft))
				css += " datetimepicker-left";
			else if (Options.HasFlag(InputDateTimeOptions.PopoutRight))
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

	public InputDateTime()
	{
		var nullable = Nullable.GetUnderlyingType(typeof(TValue));

		UnderlyingType = nullable ?? typeof(TValue);
		IsNullable = nullable != null;

		if (UnderlyingType != typeof(DateTime) && UnderlyingType != typeof(DateOnly) && UnderlyingType != typeof(TimeSpan) && UnderlyingType != typeof(TimeOnly))
			throw new InvalidOperationException($"Unsupported type param '{UnderlyingType.Name}'. Must be of type {nameof(DateTime)}, {nameof(DateOnly)}, {nameof(TimeSpan)}, or {nameof(TimeOnly)}.");
	}

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		// Set popout value
		UpdatePopoutValue();
	}

	/// <inheritdoc />
	protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	protected override string FormatValueAsString(TValue? value) => value switch
	{
		DateTime dateTimeValue => FormatDateTime(dateTimeValue),
		DateOnly dateOnlyValue => FormatDateOnly(dateOnlyValue),
		TimeSpan timeSpanValue => FormatTimeSpan(timeSpanValue),
		TimeOnly timeOnlyValue => FormatTimeOnly(timeOnlyValue),
		_ => string.Empty
	};

	private string FormatDateTime(DateTime value)
	{
		throw new NotImplementedException();
	}

	private string FormatDateOnly(DateOnly value) => FormatDateTime(value.ToDateTime(TimeOnly.MinValue));

	private string FormatTimeSpan(TimeSpan value)
	{
		throw new NotImplementedException();
	}

	private string FormatTimeOnly(TimeOnly value) => FormatTimeSpan(value.ToTimeSpan());

	private void CheckKeyPress(KeyboardEventArgs args)
	{
		if (args.Key == "Escape" || args.Key == "Tab")
			ClosePopout();
	}

	private void OpenPopout()
	{
		if (IsPopoutDisplayed || Options.HasFlag(InputDateTimeOptions.NoPopout))
			return;

		IsPopoutDisplayed = true;

		if (Options.HasFlag(InputDateTimeOptions.UseAutomaticStatusColors))
			ResetStatus();
	}

	private void ClosePopout(bool save = false, bool reset = false)
	{
		if (IsPopoutDisplayed == false || Options.HasFlag(InputDateTimeOptions.NoPopout))
			return;

		IsPopoutDisplayed = false;

		if (reset)
			PopoutValue = DateTime.Now;

		if (save || reset)
			CurrentValueAsString = FormatDateTime(PopoutValue);
	}

	private void OnChange(ChangeEventArgs args)
	{
		CurrentValueAsString = args.Value?.ToString();
		UpdatePopoutValue();
	}

	private void UpdatePopoutValue()
	{
		if (IsNullable && Value == null)
			PopoutValue = DateTime.Now;
		else if (UnderlyingType == typeof(DateTime))
			PopoutValue = (DateTime)Convert.ChangeType(Value!, typeof(DateTime));
		else if (UnderlyingType == typeof(DateOnly))
			PopoutValue = ((DateOnly)(object)Value!).ToDateTime(TimeOnly.MinValue);
		else if (UnderlyingType == typeof(TimeSpan))
			PopoutValue = DateTime.Today.Add((TimeSpan)(object)Value!);
		else
			PopoutValue = DateOnly.FromDateTime(DateTime.Today).ToDateTime((TimeOnly)(object)Value!);
	}

	private void UpdatePopoutValue(TimeSpan adjustment)
	{

	}

	private void ResetStatus()
	{
		DisplayStatus &= ~InputStatus.BackgroundDanger;
		DisplayStatus &= ~InputStatus.BackgroundWarning;
		DisplayStatus &= ~InputStatus.BackgroundSuccess;
	}
}
