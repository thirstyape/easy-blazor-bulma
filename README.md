# Easy Blazor Bulma

[![MIT](https://img.shields.io/github/license/thirstyape/Easy-Blazor-Bulma)](https://github.com/thirstyape/Easy-Blazor-Bulma/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Easy.Blazor.Bulma.svg)](https://www.nuget.org/packages/Easy.Blazor.Bulma/)

## Getting Started

### Installation

To use this library either clone a copy of the repository or check out the [NuGet package](https://www.nuget.org/packages/Easy.Blazor.Bulma/)

### Usage

**Basic Example**

The following example provides a complete use case. This example makes use of the most basic configuration.

In your index.html file add the following line.

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
