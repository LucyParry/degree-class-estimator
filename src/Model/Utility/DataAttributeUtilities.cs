using System;
using System.Linq;

namespace DegreeClassEstimator.Model
{
    public static class DataAttributeUtilities
    {
        public static Attr GetAttribute<Typ, Attr>(this Typ aObject, string propertyName) where Attr : Attribute
        {
            Attr attr = (Attr)aObject.GetType()
                .GetProperties()
                .SingleOrDefault(x => x.Name == propertyName)
                .GetCustomAttributes(typeof(Attr), false)
                .SingleOrDefault();

            return attr;
        }
    }
}
