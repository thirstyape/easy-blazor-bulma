﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace easy_blazor_bulma;

/// <summary>
/// Simple responsive horizontal navigation tabs, with different styles.
/// </summary>
/// <remarks>
/// <see href="https://bulma.io/documentation/components/tabs/">Bulma Documentation</see>
/// </remarks>
public partial class Tabs : ComponentBase
{
	/// <summary>
	/// The name of the tab that is currently displayed.
	/// </summary>
	[Parameter]
	public string? Active { get; set; }

	/// <summary>
	/// Event that occurs when <see cref="Active"/> is modified.
	/// </summary>
	[Parameter]
	public EventCallback<string?>? ActiveChanged { get; set; }

    /// <summary>
    /// Positions the tab bar in the center of the area when true.
    /// </summary>
    [Parameter]
	public bool IsCentered { get; set; } = true;

    /// <summary>
    /// Uses the more classic style with borders for elements in the tab bar.
    /// </summary>
    [Parameter]
    public bool IsBoxed { get; set; }

    /// <summary>
    /// Uses mutually exclusive tabs (like radio buttons) for elements in the tab bar.
    /// </summary>
    [Parameter]
    public bool IsToggle { get; set; } = true;

    /// <summary>
    /// Rounds the first and last elements in the tab bar. Requires <see cref="IsToggle"/> to be true.
    /// </summary>
    [Parameter] 
	public bool IsRounded { get; set; } = true;

    /// <summary>
    /// Event that occurs when an item in the tab bar is clicked.
    /// </summary>
    [Parameter]
    public Func<string?, Task>? OnItemClicked { get; set; }

    /// <summary>
    /// The content to display within the tab bar. Can contain <see cref="Tab"/> elements, as well as other components and markup.
    /// </summary>
    [Parameter]
	public RenderFragment? ChildContent { get; set; }

	/// <summary>
	/// Any additional attributes applied directly to the component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object>? AdditionalAttributes { get; set; }

	[Inject]
	private IServiceProvider ServiceProvider { get; init; }

	private readonly List<Tab> Children = new();
	private ILogger<Tabs>? Logger;

	private string FullCssClass 
	{
		get
		{
            var css = "tabs is-size-6";

			if (IsCentered)
				css += " is-centered";

			if (IsBoxed)
				css += " is-boxed";
			else if (IsToggle)
				css += " is-toggle";

			if (IsToggle && IsRounded)
				css += " is-toggle-rounded";

            return string.Join(' ', css, CssClass);
        }
	}

	private string? CssClass
	{
		get
		{
			if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("class", out var css) && string.IsNullOrWhiteSpace(Convert.ToString(css, CultureInfo.InvariantCulture)) == false)
				return css.ToString();

			return null;
		}
	}

	/// <inheritdoc/>
	protected async override Task OnInitializedAsync()
	{
		Logger = ServiceProvider.GetService<ILogger<Tabs>>();

		if (string.IsNullOrWhiteSpace(Active) && Children.Count > 0)
		{
			Active = Children.First().Name;

			if (ActiveChanged.HasValue && ActiveChanged.Value.HasDelegate)
				await ActiveChanged.Value.InvokeAsync(Active);
		}
	}

	internal async Task AddChild(Tab tab)
	{
		if (tab.Name == null)
		{
			Logger?.LogError("Tabs must have a name assigned.");
			return;
		}
		else if (Children.Any(x => x.Name == tab.Name))
		{
			Logger?.LogError("Tabs must have a unique name. Duplicate is {name}.", tab.Name);
			return;
		}

		tab.Index = Children.Count != 0 ? Children.Max(x => x.Index) + 1 : 0;

		Children.Add(tab);

		if (string.IsNullOrWhiteSpace(Active))
		{
			Active = tab.Name;

			if (ActiveChanged.HasValue && ActiveChanged.Value.HasDelegate)
				await ActiveChanged.Value.InvokeAsync(Active);
		}

		StateHasChanged();
	}

	internal async Task RemoveChild(Tab tab)
	{
		var child = Children.FirstOrDefault(x => x.Index == tab.Index);

		if (child == null)
		{
			Logger?.LogError("Could not find tab to remove with name {name}.", tab.Name);
			return;
		}

		Children.Remove(child);

		var i = 0;

		foreach (var item in Children.OrderBy(x => x.Index))
			item.Index = i++;

		if (Active == child.Name && Children.Count != 0)
		{
			Active = Children.First().Name;

			if (ActiveChanged.HasValue && ActiveChanged.Value.HasDelegate)
				await ActiveChanged.Value.InvokeAsync(Active);
		}

		StateHasChanged();
	}

	private async Task OnSelectionChanged(Tab tab)
	{
		if (tab.AdditionalAttributes != null && tab.AdditionalAttributes.Any(x => x.Key == "disabled" && (x.Value.ToString() == "disabled" || x.Value.ToString() == "true"))) 
			return;

        if (OnItemClicked != null)
            await OnItemClicked.Invoke(tab.Name);

        if (Active != tab.Name)
		{
			Active = tab.Name;

			if (ActiveChanged.HasValue && ActiveChanged.Value.HasDelegate)
				await ActiveChanged.Value.InvokeAsync(Active);
		}
	}

	private string? GetChildCssClass(Tab tab)
	{
		string css;

		if (tab.Index == 0)
			css = "mr-1 ";
		else if (tab.Index == Children.Count - 1)
			css = "ml-1 ";
		else
			css = "mx-1 ";

		if (Active == tab.Name)
			css += "is-active";

		if (tab.AdditionalAttributes != null && tab.AdditionalAttributes.TryGetValue("class", out var tabCss) && string.IsNullOrWhiteSpace(Convert.ToString(tabCss, CultureInfo.InvariantCulture)) == false)
			css += $" {tabCss}";

		return css.TrimEnd();
	}
}
