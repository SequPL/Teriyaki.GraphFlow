using System;
using System.Collections.Generic;
using System.Text;

namespace Teriyaki.GraphFlow
{
    public class NodeInputAttribute : Attribute
    {
        public object? DefaultValue { get; set; }
    }
}
