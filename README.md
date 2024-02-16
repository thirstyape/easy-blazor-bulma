# Easy Blazor Bulma

[![MIT](https://img.shields.io/github/license/thirstyape/Easy-Blazor-Bulma)](https://github.com/thirstyape/Easy-Blazor-Bulma/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Easy.Blazor.Bulma.svg)](https://www.nuget.org/packages/Easy.Blazor.Bulma/)

## Getting Started

This library provides an easy to use, out of the box implementation of the Bulma CSS framework for use with Blazor. It uses the latest version of Bulma, so you do not need to worry about updating it yourself. There are also several extensions to Bulma that are included and integrated here. Additionally, the Google Material Symbols font is packaged here also. All of this is takes just a single line in your index.html or single using statement in your CS files to start using.

After install, you can use all of the Bulma styles, along with those of the extension packages on your own components. You may want to store a copy of the pre-compiled CSS files somewhere in your solution so that Visual Studio can use Intellisense to give you CSS class recommendations. You will also be able to use the `.material-icons` CSS class to use any of the Material Symbols. The icon font will also be updated regularly, just let me know if there are any missing and I can do an update.

Finally, you can use any of the pre-built components here to simplify use of the Bulma framework. Items such as `<Tabs />`, `<Panel />`, or	`<ProgressBar />` will all be available. There is a list below, eventually all items in the Bulma documentation will be added. If you need one of the missing ones either do a Pull Request or let me know and I may have time.

### Installation

To use this library either clone a copy of the repository or check out the [NuGet package](https://www.nuget.org/packages/Easy.Blazor.Bulma/)

### Features

* Pre-compiled CSS bundle containing Bulma, several extensions, and a dark theme
* Interactive Blazor components for Bulma framework elements
* Bundled with Google Material Symbols icon font
* No additional dependencies (Microsoft.AspNetCore.Components.Web is already required by Blazor)
* No JavaScript required (except for `<ThemeSelector />`)

### Component List

The following components are currently available for use. All of them are documented and you can hover over them in Visual Studio to get details for the components and their parameters.

**Components**

* `<Message />`, creates a styled block containing a note to display
* `<Modal />`, creates an overlay that can display additional content
* `<Panel />`, creates a styled block containing content to display
* `<Steps />` and `<Step />`, creates a progress tracker with visual indicators
* `<Tabs />` and `<Tab />`, creates a tab bar and content panels

**Elements**

* `<BooleanIcon />`, creates an icon with either a check or 'X'

**Form**

* `<InputCharacter />`, creates a series of buttons to select a single character
* `<InputDateTime />`, creates an input with a popout for binding date and time values
* `<InputDuration />`, creates an input with a popout for binding duration values
* `<InputFlaggedEnum />`, creates a series of checkboxes for bitmasked enum values
* `<InputNumberPad />`, creates a keyboard style number pad for numeric values

**Helpers**

* `<ThemeSelector />`, toggles between dark and light CSS themes (requires JavaScript file and both CSS stylesheets in index.html)
* `<TitleBlock />`, displays a banner accross the top of the screen with some text

### Usage

**Basic Example**

The following example provides a complete use case. This example makes use of the most basic configuration.

In your index.html file add the following line. You must do this even if you just want to use the components, they need this too.

```html
<link rel="stylesheet" href="_content/Easy.Blazor.Bulma/css/easy-blazor-bulma.min.css" />
```

This will link the CSS style pack and Google Material Symbols icon font.

Next, add one of the components or use some of the styles in your own components.

```razor
@using easy_blazor_bulma

<p class='block'>Here's a giant checkbox!</p>
<BooleanIcon class='is-size-1 is-jumbo' @bind-Value=Test />

@code {
	private bool Test { get; set; } = true;
}
```

**Dark Theme Example**

To use the dark theme, just add a secondary stylesheet reference in your index.html. The theme can automatically be selected based on the preference of the user. Changing between the styles can be done easily with an `IJSRuntime`.

```razor
<head>
    <!-- Your head content -->
    <link rel="stylesheet" href="_content/Easy.Blazor.Bulma/css/easy-blazor-bulma.min.css" id="easy-blazor-bulma" media="(prefers-color-scheme: light)" />
    <link rel="stylesheet" href="_content/Easy.Blazor.Bulma/css/easy-blazor-bulma-dark.min.css" id="easy-blazor-bulma-dark" media="(prefers-color-scheme: dark)" />
</head>

<body>
    <!-- Your body content -->
    <script src="_content/Easy.Blazor.Bulma/js/easy-blazor-bulma.js" type="text/javascript"></script>
</body>
```

There are 5 helper methods in the bundled JavaScript file.

* easyBlazorBulma.IsOsDarkMode, checks to see if dark mode is currently enabled
* easyBlazorBulma.ToggleStyleSheet, enables or disables the element with the matching id
* easyBlazorBulma.HasStorage, tests to see if browser storage is available
* easyBlazorBulma.WriteStorage, saves the provided value to browser storage
* easyBlazorBulma.ReadStorage, retrieves the specified value from browser storage

Additionally, you can use the `<ThemeSelector />` component to display a button that will toggle between themes. By default it applies the `.navbar-item` CSS class, if you provide another class this will override it. Simply including this component anywhere in your display will automatically load the correct theme when your app starts.

## Build Details

### Frameworks

- .NET 6.0
- .NET 7.0
- .NET 8.0

### External Libraries

| Name | Usage | License | Library |
| ------ | ------ | ------ | ------ |
| Bulma | Submodule | MIT | [GitHub](https://github.com/jgthms/bulma) |
| Bulma Calendar | Submodule | MIT | [GitHub](https://github.com/Wikiki/bulma-calendar) |
| Bulma Checkradio | Submodule | MIT | [GitHub](https://github.com/Wikiki/bulma-checkradio) |
| Bulma-O-Steps | Submodule | GPL-3.0 | [GitHub](https://github.com/octoshrimpy/bulma-o-steps) |
| Bulma Switch  | Submodule | MIT | [GitHub](https://github.com/Wikiki/bulma-switch) |
| Bulma Tooltip | Submodule | MIT | [GitHub](https://github.com/CreativeBulma/bulma-tooltip) |
| Bulma Jumbo Tiny | Submodule | MIT | [GitHub](https://github.com/thirstyape/Bulma-Jumbo-Tiny) |
| Material Symbols | Embedded | Apache 2.0 | [GitHub](https://github.com/google/material-design-icons) |

## Authors

* **NF Software Inc.**

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

Thank you to:
* [Kmg Design](https://www.iconfinder.com/kmgdesignid) for the project icon
