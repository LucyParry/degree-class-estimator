using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Model.Utility
{
    public static class DataAttributeUtilities
    {
        public static Attr GetStringLengthAttribute<Type, Attr>(this Type aObject, string propertyName) where Attr: Attribute
        {
            Attr attribute = (Attr)aObject.GetType()
                .GetProperties()
                .SingleOrDefault(x => x.Name == propertyName)
                .GetCustomAttributes(typeof(Attr), false)
                .SingleOrDefault();

            return attribute;
}
    }
}
