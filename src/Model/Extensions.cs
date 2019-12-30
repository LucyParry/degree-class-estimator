using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace HonoursClassEstimator.Model
{
    public static class Extensions
    {
        /// <summary>
        /// Get the 'Description' attribute of an Enum
        /// </summary>
        public static string GetEnumDescription<T>(this T enumVal) where T: IConvertible
        {
            string description = null;
            Type type = enumVal.GetType();
            if (type.IsEnum)
            {
                var memberInfo = type.GetMember(enumVal.ToString());
                if (memberInfo.Length > 0)
                {
                    var customAttribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (customAttribs.Length > 0)
                    {
                        description = ((DescriptionAttribute)customAttribs[0]).Description;
                    }
                }
            }          
            return description;
        }

        public static string Base64Encode(this string baseString, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(baseString))
            {
                return baseString;
            }
            byte[] bytes = encoding.GetBytes(baseString);
            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(this string encodedString, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(encodedString))
            {
                return encodedString;
            }
            byte[] bytes = Convert.FromBase64String(encodedString);
            return encoding.GetString(bytes);
        }
    }
}
