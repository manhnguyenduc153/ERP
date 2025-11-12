using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ERP_API.Enums;
using System.Reflection;

namespace ERP_API.Authorization
{
    public static class PermissionHelper
    {
        public static List<(string Key, string DisplayName)> GetAllPermissions()
        {
            return Enum.GetValues(typeof(Permission))
                .Cast<Permission>()
                .Select(p => (p.ToString(), GetDisplayName(p)))
                .ToList();
        }

        public static string GetDisplayName(Enum value)
        {
            return value.GetType()
                        .GetField(value.ToString())
                        ?.GetCustomAttribute<DescriptionAttribute>()?.Description
                        ?? value.ToString();
        }
    }
}
