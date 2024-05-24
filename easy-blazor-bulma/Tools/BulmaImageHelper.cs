namespace easy_blazor_bulma;

/// <summary>
/// Contains methods to assist with use of the images within Bulma.
/// </summary>
public static class BulmaImageHelper
{
    /// <summary>
    /// Converts the specified Bulma size to its CSS class.
    /// </summary>
    /// <param name="size">The size to convert.</param>
    public static string GetImageCss(BulmaImageSizes size) => size switch
    {
        BulmaImageSizes.Is16x16 => "is-16x16",
        BulmaImageSizes.Is24x24 => "is-24x24",
        BulmaImageSizes.Is32x32 => "is-32x32",
        BulmaImageSizes.Is48x48 => "is-48x48",
        BulmaImageSizes.Is64x64 => "is-64x64",
        BulmaImageSizes.Is96x96 => "is-96x96",
        BulmaImageSizes.Is128x128 => "is-128x128",
        BulmaImageSizes.Is192x192 => "is-192x192",
        BulmaImageSizes.Is256x256 => "is-256x256",
        BulmaImageSizes.Is384x384 => "is-384x384",
        BulmaImageSizes.Is512x512 => "is-512x512",
        BulmaImageSizes.Is768x768 => "is-768x768",
        BulmaImageSizes.Is1024x1024 => "is-1024x1024",

        BulmaImageSizes.IsSquare => "is-square",
        BulmaImageSizes.Is1by1 => "is-1by1",
        BulmaImageSizes.Is5by4 => "is-5by4",
        BulmaImageSizes.Is4by3 => "is-4by3",
        BulmaImageSizes.Is3by2 => "is-3by2",
        BulmaImageSizes.Is5by3 => "is-5by3",
        BulmaImageSizes.Is16by9 => "is-16by9",
        BulmaImageSizes.Is2by1 => "is-2by1",
        BulmaImageSizes.Is3by1 => "is-3by1",
        BulmaImageSizes.Is4by5 => "is-4by5",
        BulmaImageSizes.Is3by4 => "is-3by4",
        BulmaImageSizes.Is2by3 => "is-2by3",
        BulmaImageSizes.Is3by5 => "is-3by5",
        BulmaImageSizes.Is9by16 => "is-9by16",
        BulmaImageSizes.Is1by2 => "is-1by2",
        BulmaImageSizes.Is1by3 => "is-1by3",
        _ => string.Empty
    };
}
