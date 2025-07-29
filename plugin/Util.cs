using System;
using System.Linq;
using System.Reflection;

namespace SCPlus.plugin
{
    internal static class Util
    {
        internal static TEnum[] GetValues<TEnum>() where TEnum : Enum
        {
            return [.. typeof(TEnum)
                    .GetFields(BindingFlags.Static | BindingFlags.Public)
                    .Select(fieldInfo => (TEnum)fieldInfo.GetValue(null))];
        }
        
        internal static bool HasFlag(this Enum value, Enum flag)
        {
            return (Convert.ToInt64(value) & Convert.ToInt64(flag)) != 0;
        }
    }
}
