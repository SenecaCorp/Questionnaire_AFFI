using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Entities.Infrastructure
{
    public static class EnumHelper
    {
        public static TEnum ParseEnum<TEnum>(string value)
        {
            if (value == null)
            {
                return default(TEnum);
            }

            Type enumType = typeof(TEnum);
            if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                enumType = Nullable.GetUnderlyingType(enumType);
            }

            if (Enum.IsDefined(enumType, value))
                return (TEnum)Enum.Parse(enumType, value);
            else return default(TEnum);
        }

        public static string EnumName<TEnum>(TEnum value)
        {
            if (value == null || !(value is Enum))
            {
                return null;
            }

            Type enumType = typeof(TEnum);
            if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                enumType = Nullable.GetUnderlyingType(enumType);
            }
            
            return Enum.GetName(enumType, value);
        }
    }
}