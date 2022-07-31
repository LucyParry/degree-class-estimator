using System;
using System.ComponentModel;
using System.Reflection;

namespace DegreeClassEstimator.Model
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the 'Description' attribute of an Enum
        /// </summary>
        public static string GetEnumDescription<T>(this T enumVal) where T : Enum
        {
            string description = null;
            var members = typeof(T).GetMember(enumVal.ToString());
            if (members.Length > 0)
            {
                DescriptionAttribute attribute = members[0].GetCustomAttribute<DescriptionAttribute>();
                description = attribute is null ? null : attribute.Description;
            }
            return description;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetDisplayName<T>(this T enumVal) where T : Enum
        {
            string description = enumVal.GetEnumDescription();
            if (description is not null)
            {
                return description;
            }
            return enumVal.ToString();
        }
    }
}
