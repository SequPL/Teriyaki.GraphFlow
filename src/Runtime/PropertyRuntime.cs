using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teriyaki.GraphFlow.Models.Graph.Attributes;

namespace Teriyaki.GraphFlow.Runtime
{
    public class PropertyRuntime : IPropertyAccessor
    {
        public string Id => Node.Node.GetPropertyId(Name);
        public Func<object> GetValue { get; init; }
        public Func<object, object> SetValue { get; init; }
        public string Name { get; init; }
        public INodeRuntime Node { get; init; }
        public NodePropertyAttribute? PropertyAttribute { get; init; }
    }
}
