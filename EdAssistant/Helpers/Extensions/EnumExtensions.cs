using EdAssistant.Translations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EdAssistant.Helpers.Extensions;

public static class EnumExtensions
{
    public static string GetLocalizedDisplayName(this Enum enumValue)
    {
        var displayAttribute = enumValue.GetType()
            .GetMember(enumValue.ToString())[0]
            .GetCustomAttribute<DisplayAttribute>();

        if (displayAttribute?.Name is not null)
        {
            return Localization.Instance[displayAttribute.Name];
        }

        return enumValue.ToString();
    }
}