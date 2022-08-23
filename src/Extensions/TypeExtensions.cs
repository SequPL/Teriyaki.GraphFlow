using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teriyaki
{
    public static class TypeExtensions
    {
        public static Type GetGenericTypeFromInterface(this Type @this, Type interfaceType)
        {
            if (!interfaceType.IsGenericTypeDefinition) throw new ArgumentOutOfRangeException($"{interfaceType.Name} have to be generic type.");
            //
            var interfaces = @this.GetInterfaces();
            var outInterface = interfaces.FirstOrDefault(q => q.IsConstructedGenericType && q.GetGenericTypeDefinition() == interfaceType);
            //
            if (outInterface == null) return null;
            //
            var gargs = outInterface.GetGenericArguments();
            var outputType = gargs[0];
            //
            return outputType;
        }
    }
}
