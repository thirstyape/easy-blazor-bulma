using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// Provides a button that toggles between the light and dark Bulma CSS themes.
/// </summary>
public partial class ThemeSelector : ComponentBase
{
	/// <summary>
	/// Applies the theme saved by the user on component initialization when true.
	/// </summary>
	[Parameter]
	public bool LoadUserPreference { get; set; } = true;

	/// <summary>
	/// The HTML id attribute assigned to the light Easy Blazor Bulma CSS stylesheet.
	/// </summary>
	[Parameter]
	public string LightThemeId { get; set; } = "easy-blazor-bulma";

	/// <summary>
	/// The HTML id attribute assigned to the dark Easy Blazor Bulma CSS stylesheet.
	/// </summary>
	[Parameter]
	public string DarkThemeId { get; set; } = "easy-blazor-bulma-dark";

	/// <summary>
	/// The keyname to store the user theme preference under.
	/// </summary>
	[Parameter]
	public string DarkModeKeyName { get; set; } = "easyblazorbulma.isdarkmode";

	/// <summary>
	/// Any additional attributes applied directly to the component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object>? AdditionalAttributes { get; set; }

	[Inject]
	private IJSRuntime JsRuntime { get; init; }

	private string FullCssClass => string.IsNullOrWhiteSpace(CssClass) == false ? CssClass : "navbar-item";

	private string? CssClass
	{
		get
		{
			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("class", out var css) && string.IsNullOrWhiteSpace(Convert.ToString(css, CultureInfo.InvariantCulture)) == false)
				return css.ToString();

			return null;
		}
	}

	private bool IsDarkMode;
	private string Icon => IsDarkMode ? "dark_mode" : "light_mode";

	/// <inheritdoc />
	protected async override Task OnInitializedAsync()
	{
		var isOsDarkMode = await JsRuntime.InvokeAsync<bool>("easyBlazorBulma.IsOsDarkMode");

		if (LoadUserPreference == false)
		{
			IsDarkMode = isOsDarkMode;
			return;
		}

		var isUserDarkMode = await JsRuntime.InvokeAsync<string>("easyBlazorBulma.ReadStorage", DarkModeKeyName);

		if (string.IsNullOrWhiteSpace(isUserDarkMode))
		{
			IsDarkMode = isOsDarkMode;
			return;
		}

		var isDarkMode = isUserDarkMode.Equals("true", StringComparison.OrdinalIgnoreCase);

		if (isDarkMode != isOsDarkMode)
		{
			if (isOsDarkMode)
				await SetActiveTheme(LightThemeId, DarkThemeId);
			else
				await SetActiveTheme(DarkThemeId, LightThemeId);
		}

		IsDarkMode = isDarkMode;
	}

	private async Task ToggleMode()
	{
		if (IsDarkMode)
			await SetActiveTheme(LightThemeId, DarkThemeId);
		else
			await SetActiveTheme(DarkThemeId, LightThemeId);

		IsDarkMode = !IsDarkMode;
		await JsRuntime.InvokeAsync<bool>("easyBlazorBulma.WriteStorage", DarkModeKeyName, IsDarkMode.ToString());
	}

	private async Task SetActiveTheme(string activate, string deactivate)
	{
		await JsRuntime.InvokeAsync<bool>("easyBlazorBulma.ToggleStyleSheet", activate, true, true);
		await JsRuntime.InvokeAsync<bool>("easyBlazorBulma.ToggleStyleSheet", deactivate, false);
	}
}
