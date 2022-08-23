using System;
using System.Collections.Generic;
using System.Text;
using Teriyaki.GraphFlow.Models.Graph.Attributes;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;

namespace Teriyaki.GraphFlow.Nodes
{
    public class ConstNumberNode : GraphNodeModel<ConstNumberNode, GraphNodeBasicInputs, ConstNumberNode.Outputs, ConstNumberNode.Properties>
    {
        public class Properties : GraphNodeBasicProperties
        {
            public long Value { get; set; }
        }

        public class Outputs : GraphNodeBasicOutputs
        {
            public long Value { get; set; }
        }
    }
}
