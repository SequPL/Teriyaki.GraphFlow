using System;
using System.Collections.Generic;
using System.Reflection;

namespace Teriyaki.GraphFlow.Runtime
{
    public abstract record LinkSlotRuntime : IPropertyAccessor
    {
        public abstract string Id { get; }
        public string Name { get; init; }

        public INodeRuntime Node { get; init; }
        //
        //public PropertyInfo Property { get; init; }
        public Func<object> GetValue { get; init; }
        public Func<object, object> SetValue { get; init; }

        public List<ILinkRuntime> Links { get; set; } = new List<ILinkRuntime>();
    }

    public interface IPropertyAccessor
    {
        string Id { get; }
        Func<object> GetValue { get;  }
        Func<object, object> SetValue { get;  }
    }
}