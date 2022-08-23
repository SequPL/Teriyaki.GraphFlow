using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;

namespace Teriyaki.GraphFlow.Nodes
{
    public class InputValueNode : GraphNodeModel<InputValueNode, InputValueNode.Inputs, InputValueNode.Outputs, InputValueNode.Properties>
    {
        public class Inputs : GraphNodeBasicInputs
        {
            public string Value { get; set; }
        }
        public class Outputs : GraphNodeBasicOutputs
        {
            public string Value { get; set; }
        }
        public class Properties : GraphNodeBasicProperties
        {
            public bool IsExecuted { get; set; }
        }
    }
}
