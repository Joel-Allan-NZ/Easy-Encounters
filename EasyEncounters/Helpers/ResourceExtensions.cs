﻿using Microsoft.Windows.ApplicationModel.Resources;

namespace EasyEncounters.Helpers;

public static class ResourceExtensions
{
    private static readonly ResourceLoader _resourceLoader = new();

    public static string GetEnumerationString(Enum enumeration)
    {
        var resourceName = string.Concat(enumeration.GetType().Name, "_", enumeration, "_Short");
        return GetLocalized(resourceName);
    }

    public static string GetEnumerationDescription(Enum enumeration)
    {
        var resourceName = string.Concat(enumeration.GetType().Name, "_", enumeration, "_Description");
        return GetLocalized(resourceName);
    }

    public static string GetLocalized(this string resourceKey) => _resourceLoader.GetString(resourceKey);
}