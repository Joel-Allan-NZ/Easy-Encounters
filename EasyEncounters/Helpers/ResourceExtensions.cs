using Microsoft.Windows.ApplicationModel.Resources;

namespace EasyEncounters.Helpers;

public static class ResourceExtensions
{
    private static readonly ResourceLoader _resourceLoader = new();

    public static string GetLocalized(this string resourceKey) => _resourceLoader.GetString(resourceKey);

    public static string GetEnumerationString(Enum enumeration)
    {
        string resourceName = string.Concat(enumeration.GetType().Name, "_", enumeration);
        return GetLocalized(resourceName);
    }
}
