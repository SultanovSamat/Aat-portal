using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AkfortaWeb.Helpers
{
   public static class EnumHelper
   {
      public static string GetDescription(this Enum value)
      {
         var field = value.GetType().GetField(value.ToString());
         if (field == null) return value.ToString();
         return !(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            ? value.ToString()
            : attribute.Description;
      }

      public static string GetEnumDescription<T>(this string value)
      {
         var type = typeof(T);
         return value.GetEnumDescription(type);
      }
      
      public static string GetEnumDescription(this string value, Type enumType)
      {
         if (!enumType.IsEnum) throw new ArgumentException("Type must be enum", nameof(enumType));

         var field = enumType.GetField(value);
         return !(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            ? value
            : attribute.Description;
      }

      public static KeyValuePair<string, string>[] GetEnumNamesDescriptions<T>() where T : Enum
      {
         var type = typeof(T);
         return GetEnumNamesDescriptions(type);
      }

      public static KeyValuePair<string, string>[] GetEnumNamesDescriptions(this Type type)
      {
         if (!type.IsEnum) throw new ArgumentException("Type must be enum", nameof(type));

         var names = Enum.GetNames(type);
         return names.Select(name => new KeyValuePair<string, string>(name, name.GetEnumDescription(type))).ToArray();
      }
   }
}