using System;
using System.Collections.Generic;
using System.Text;
using Teriyaki.GraphFlow.Models.Graph.Attributes;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;

namespace Teriyaki.GraphFlow.Nodes
{
    public class ButtonNode : GraphNodeModel<ButtonNode, GraphNodeBasicInputs, ButtonNode.Outputs>
    {
        public bool IsClicked { get; set; }
        
        public class Outputs : GraphNodeBasicOutputs
        {
            public bool IsClicked { get; set; }
        }
    }
}