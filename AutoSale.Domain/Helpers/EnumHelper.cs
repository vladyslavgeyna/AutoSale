using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AutoSale.Domain.Helpers
{
    public static class EnumHelper
    {
        public static string GetEnumDisplayName(System.Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();
        
            if (attribute is not null && !string.IsNullOrWhiteSpace(attribute.Name))
            {
                return attribute.Name;
            }

            return value.ToString();
        }

    }
}