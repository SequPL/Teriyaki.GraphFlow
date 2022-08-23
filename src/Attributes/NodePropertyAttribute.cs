using System;
using System.Collections.Generic;
using System.Text;

namespace Teriyaki.GraphFlow.Models.Graph.Attributes
{
    public class NodePropertyAttribute : Attribute
    {
        public object? DefaultValue { get; set; }
    }
}
