using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;

namespace Teriyaki.GraphFlow.Nodes
{
    public class IfNode : GraphNodeModel<IfNode, GraphNodeBasicInputs, IfNode.Outputs, IfNode.Properties>
    {
        public class Properties : GraphNodeBasicProperties
        {
            public Expression Condition { get; set; }
        }

        public class Outputs : GraphNodeBasicOutputs
        {
            public bool IsFalse { get; set; }
            public bool IsTrue { get; set; }
        }
    }
    

    public class Expression
    {
        public string Value { get; set; }
        public static implicit operator Expression(string expression) => new(){ Value  = expression };
    }
    
}
