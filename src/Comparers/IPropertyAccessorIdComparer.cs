using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teriyaki.GraphFlow.Runtime;

namespace Teriyaki.GraphFlow.Comparers
{
    public class IPropertyAccessorIdComparer<TAccessorType> : IEqualityComparer<TAccessorType>
        where TAccessorType : IPropertyAccessor
    {
        public bool Equals(TAccessorType? x, TAccessorType? y) => x?.Id == y?.Id;

        public int GetHashCode([DisallowNull] TAccessorType obj) => obj.Id.GetHashCode();
    }
}
