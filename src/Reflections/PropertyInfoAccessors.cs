using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Teriyaki.GraphFlow.Reflections
{
    public static class PropertyInfoAccessors
    {
        public static object? SetValue(object data, PropertyInfo property, object? value)
        {
            //if (property.DeclaringType != data.GetType()) throw new AccessViolationException($"Trying set value from property {property} which has type {property.DeclaringType}, but data's type is {data.GetType()}.");
            //
            property.SetValue(data, value);
            //
            return value;
        }

        public static object? GetValue(object data, PropertyInfo property)
        {
            //if (property.DeclaringType != data.GetType()) throw new AccessViolationException($"Trying get value from property {property} which has type {property.DeclaringType}, but data's type is {data.GetType()}.");
            //
            return property.GetValue(data);
        }
    }
}
