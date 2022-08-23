using System;
using System.Collections.Generic;
using System.Text;
using Teriyaki.GraphFlow.Models.Graph.Attributes;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;

namespace Teriyaki.GraphFlow.Nodes
{
    public class ConstStringNode : GraphNodeModel<ConstStringNode, GraphNodeBasicInputs, ConstStringNode.Outputs, ConstStringNode.Properties>
    {
        public class Properties : GraphNodeBasicProperties
        {
            public string Value { get; set; }
        }

        public class Outputs : GraphNodeBasicOutputs
        {
            public string Value { get; set; }
        }
    }
}
