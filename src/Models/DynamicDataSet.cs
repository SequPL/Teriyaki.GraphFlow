using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teriyaki.GraphFlow.Models.Inputs;

namespace Teriyaki.GraphFlow.Models
{
    public class DynamicDataSet<TDynamicTypeData> : HashSet<TDynamicTypeData>
        where TDynamicTypeData : DynamicSlotBase
    {
        public class Comparer : IEqualityComparer<TDynamicTypeData>
        {
            public bool Equals(TDynamicTypeData? x, TDynamicTypeData? y)
            {
                return x.Name == y.Name;
            }

            public int GetHashCode([DisallowNull] TDynamicTypeData obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        public DynamicDataSet()
            : base(new Comparer())
        {
        }



        public TDynamicTypeData? this[string name]
        {
            get { return this.FirstOrDefault(q => q.Name == name); }
        }
    }

    public abstract class DynamicSlotBase
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Type Type { get; set; }
    }
}
