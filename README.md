# Easy Blazor Bulma

[![MIT](https://img.shields.io/github/license/thirstyape/Easy-Blazor-Bulma)](https://github.com/thirstyape/Easy-Blazor-Bulma/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Easy.Blazor.Bulma.svg)](https://www.nuget.org/packages/Easy.Blazor.Bulma/)

## Getting Started

This library provides an easy to use, out of the box implementation of the Bulma CSS framework for use with Blazor. It uses the latest version of Bulma, so you do not need to worry about updating it yourself. There are also several extensions to Bulma that are included and integrated here. Additionally, the Google Material Symbols font is packaged here also. All of this is takes just a single line in your index.html or single using statement in your CS files to start using.

After install, you can use all of the Bulma styles, along with those of the extension packages on your own components. You may want to store a copy of the pre-compiled CSS files somewhere in your solution so that Visual Studio can use Intellisense to give you CSS class recommendations. You will also be able to use the `.material-icons` CSS class to use any of the Material Symbols. The icon font will also be updated regularly, just let me know if there are any missing and I can do an update.

Finally, you can use any of the pre-built components here to simplify use of the Bulma framework. Items such as `<Tabs />`, `<Panel />`, or	`<ProgressBar />` will all be available. There is a list below, eventually all items in the Bulma documentation will be added. If you need one of the missing ones either do a Pull Request or let me know and I may have time.

### Installation

To use this library either clone a copy of the repository or check out the [NuGet package](https://www.nuget.org/packages/Easy.Blazor.Bulma/)

### Component List

The following components are currently available for use. All of them are documented and you can hover over them in Visual Studio to get details for the components and their parameters.

**Components**

* `<Tabs />` and `<Tab />`, creates a tab bar and content panels

**Elements**

* `<BooleanIcon />`, creates an icon with either a check or 'X'

**Form**

* `<InputFlaggedEnum />`, creates a series of checkboxes for bitmasked enum values

### Usage

**Basic Example**

The following example provides a complete use case. This example makes use of the most basic configuration.

In your index.html file add the following line. You must do this even if you just want to use the components, they need this too.

```
<link rel="stylesheet" href="_content/Easy.Blazor.Bulma/css/easy-blazor-bulma.css" />
```

This will link the CSS style pack and Google Material Symbols icon font.

Next, add one of the components or use some of the styles in your own components.

```
@using easy_blazor_bulma

<p class='block'>Here's a giant checkbox!</p>
<BooleanIcon class='is-size-1 is-jumbo' @bind-Value=Test />

@code {
	private bool Test { get; set; } = true;
}
```

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
