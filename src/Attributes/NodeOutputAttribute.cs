using System;
using System.Collections.Generic;
using System.Text;

namespace Teriyaki.GraphFlow
{
    public class NodeOutputAttribute : Attribute
    {
        public object DefaultValue { get; set; }
    }
}
