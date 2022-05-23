using System;
using System.ComponentModel;
using System.Text;

namespace DegreeClassEstimator.Model
{
    public static class EnumExtensions
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

    }
}
