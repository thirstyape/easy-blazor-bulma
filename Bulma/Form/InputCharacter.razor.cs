﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// An input component for editing date and time values. Supported types are <see cref="char"/>.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public partial class InputCharacter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : InputBase<TValue>
{
	/// <summary>
	/// Specifies the number of columns to divide the character selections into.
	/// </summary>
	[Parameter]
	[Range(1, 12)]
	public int Columns { get; set; } = 3;

	/// <summary>
	/// Specifies whether to display the case changing button.
	/// </summary>
	[Parameter]
	public bool ShowCaseChange { get; set; } = true;

	/// <summary>
	/// Specifies whether to round the character selection buttons.
	/// </summary>
	[Parameter]
	public bool IsRounded { get; set; } = true;

	/// <summary>
	/// Specifies whether to apply a border to the character selection buttons.
	/// </summary>
	[Parameter]
	public bool IsBordered { get; set; } = true;

	/// <summary>
	/// Specifies the color to highlight the selected value with.
	/// </summary>
	[Parameter]
	public BulmaColors ActiveColor { get; set; } = BulmaColors.Turquoise;

	/// <summary>
	/// The set of characters to generate buttons for.
	/// </summary>
	[Parameter]
	public char[] Characters { get; set; } = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

	private readonly bool IsNullable;
	private readonly Type UnderlyingType;

	private bool IsUpperCase = true;

	private string ColumnsCssClass
	{
		get
		{
			var css = "columns";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("columns-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
		}
	}

	private string ColumnCssClass
	{
		get
		{
			var css = "column";

			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("column-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
				css += $" {additional}";

			return css;
		}
	}

	public InputCharacter()
	{
		var nullable = Nullable.GetUnderlyingType(typeof(TValue));

		UnderlyingType = nullable ?? typeof(TValue);
		IsNullable = nullable != null;

		if (UnderlyingType != typeof(char))
			throw new InvalidOperationException($"Unsupported type param '{UnderlyingType.Name}'. Must be of type char.");
	}

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		var current = CurrentValueAsString?.FirstOrDefault();

		if (current != null && current != '\0' && char.IsUpper(current.Value) == false)
			IsUpperCase = false;
	}

	/// <inheritdoc />
	protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
	{
		// Prevent null reference
		if (IsNullable == false && string.IsNullOrWhiteSpace(value))
			value = string.Empty;

		// Try parse
		if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result))
		{
			validationErrorMessage = null;
			return true;
		}
		else
		{
			validationErrorMessage = string.Format(CultureInfo.InvariantCulture, "The {0} field must be a char.", DisplayName ?? FieldIdentifier.FieldName);
			return false;
		}
	}

	/// <inheritdoc />
	protected override string FormatValueAsString(TValue? value) => value switch
	{
		char charValue => charValue.ToString(),
		_ => string.Empty
	};

	private void OnCharacterClicked(char character)
	{
		var current = CurrentValueAsString?.FirstOrDefault();

		if (IsNullable == false && current != null && current != '\0' && char.ToUpper(character) == char.ToUpper(current.Value))
			return;
		else if (character == '\0' || (current != null && current != '\0' && char.ToUpper(character) == char.ToUpper(current.Value)))
			CurrentValueAsString = null;
		else if ((IsUpperCase && char.IsUpper(character)) || (IsUpperCase == false && char.IsLower(character)))
			CurrentValueAsString = character.ToString();
		else if (IsUpperCase)
			CurrentValueAsString = char.ToUpper(character).ToString();
		else
			CurrentValueAsString = char.ToLower(character).ToString();
	}

	private void OnKeyDown(KeyboardEventArgs args)
	{
		if (args.Key != "ArrowDown" && args.Key != "ArrowUp" && args.Key != "ArrowLeft" && args.Key != "ArrowRight")
			return;

		var current = CurrentValueAsString?.FirstOrDefault();

		if (current == null || current == '\0')
			return;

		var columns = Characters
			.Split(Columns)
			.Select((Options, IndexC) => new { IndexC, Options = Options.Select((Value, IndexR) => new { IndexR, Value }).ToList() })
			.ToList();

		var column = columns.FirstOrDefault(x => x.Options.Any(y => char.ToUpper(y.Value) == char.ToUpper(current.Value)));

		if (column == null)
			return;

		var row = column.Options.Single(x => char.ToUpper(x.Value) == char.ToUpper(current.Value));

		if (args.Key == "ArrowUp" && column.Options.Count > 1)
		{
			if (row.IndexR == 0)
				CurrentValueAsString = column.Options[^1].Value.ToString();
			else
				CurrentValueAsString = column.Options[row.IndexR - 1].Value.ToString();
		}
		else if (args.Key == "ArrowDown" && column.Options.Count > 1)
		{
			if (row.IndexR == column.Options.Count - 1)
				CurrentValueAsString = column.Options[0].Value.ToString();
			else
				CurrentValueAsString = column.Options[row.IndexR + 1].Value.ToString();
		}
		else if (args.Key == "ArrowLeft" && columns.Count > 1)
		{
			if (column.IndexC == 0 && columns[^1].Options.Count - 1 >= row.IndexR)
				CurrentValueAsString = columns[^1].Options[row.IndexR].Value.ToString();
			else if (column.IndexC == 0)
				CurrentValueAsString = columns[^1].Options.Last().Value.ToString();
			else if (columns[column.IndexC - 1].Options.Count - 1 >= row.IndexR)
				CurrentValueAsString = columns[column.IndexC - 1].Options[row.IndexR].Value.ToString();
			else
				CurrentValueAsString = columns[column.IndexC - 1].Options.Last().Value.ToString();
		}
		else if (args.Key == "ArrowRight" && columns.Count > 1)
		{
			if (column.IndexC == columns.Count - 1 && columns[0].Options.Count - 1 >= row.IndexR)
				CurrentValueAsString = columns[0].Options[row.IndexR].Value.ToString();
			else if (column.IndexC == columns.Count - 1)
				CurrentValueAsString = columns[0].Options.Last().Value.ToString();
			else if (columns[column.IndexC + 1].Options.Count - 1 >= row.IndexR)
				CurrentValueAsString = columns[column.IndexC + 1].Options[row.IndexR].Value.ToString();
			else
				CurrentValueAsString = columns[column.IndexC + 1].Options.Last().Value.ToString();
		}
	}

	private char GetCharacterDisplay(char character)
	{
		return IsUpperCase ? char.ToUpper(character) : char.ToLower(character);
	}

	private void OnCaseChanged() 
	{
		IsUpperCase = !IsUpperCase;

		var current = CurrentValueAsString?.FirstOrDefault();

		if (current == null || current == '\0')
			return;
		else if (IsUpperCase && char.IsLower(current.Value))
			CurrentValueAsString = char.ToUpper(current.Value).ToString();
		else if (Value != null && IsUpperCase == false && char.IsUpper(current.Value))
			CurrentValueAsString = char.ToLower(current.Value).ToString();
	}

	private string GetButtonCss(char character)
	{
		var current = CurrentValueAsString?.FirstOrDefault();
		var css = "button is-fullwidth mb-3";

		if (ActiveColor != BulmaColors.Default && character != '\0' && current != null && char.ToUpper(current.Value) == char.ToUpper(character))
			css += ' ' + BulmaColorHelper.GetColorCss(ActiveColor);

		if (IsRounded)
			css += " is-rounded";
		
		if (IsBordered)
			css += " is-bordered";

		if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("button-class", out var additional) && string.IsNullOrWhiteSpace(Convert.ToString(additional, CultureInfo.InvariantCulture)) == false)
			css += $" {additional}";

		return css;
	}
}