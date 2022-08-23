using System;
using System.Collections.Generic;
using System.Text;
using Teriyaki.GraphFlow.Models.Graph.Attributes;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;

namespace Teriyaki.GraphFlow.Nodes
{
    // Conditions.foreach(c=> if c
    // c e.q. "Inputs[0] > 1"
    public class SwitchNode : GraphNodeModel<SwitchNode, SwitchNode.Inputs, SwitchNode.Outputs, SwitchNode.Properties>
    {
        // Dynamicznie
        public class Inputs : GraphNodeBasicInputs
        {

        }
        // Dynamicznie
        public class Outputs : GraphNodeBasicOutputs
        {

        }
        // Dynamicznie
        public class Properties : GraphNodeBasicProperties
        {
            
        }
    }
}
