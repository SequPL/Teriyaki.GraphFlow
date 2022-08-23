using System;
using System.Linq;
using System.Text;
using Teriyaki.GraphFlow.Models.Graph.Litegraph;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Layout;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;

namespace Teriyaki.GraphFlow
{
    // https://github.com/derwish-pro/MyNodes.NET/tree/0b52643bf50716131cec73d2882d1d2eca07a9f4/Libs/Nodes

    public interface IGraphModel : IGraphNodeModel
    {

    }

    public class GraphModel : GraphModel<GraphModel>
    {
        public bool AllowMissingSlots { get; set; }
    }

    public class GraphModel<TNode> : GraphModel<TNode, GraphNodeBasicInputs, GraphNodeBasicOutputs, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TNode : GraphModel<TNode, GraphNodeBasicInputs, GraphNodeBasicOutputs, GraphNodeBasicProperties, GraphNodeBasicLayout>
    {
    }
    public class GraphModel<TNode, TInput> : GraphModel<TNode, TInput, GraphNodeBasicOutputs, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TNode : GraphModel<TNode, TInput, GraphNodeBasicOutputs, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TInput : INodeInputsModel, new()
    {
    }

    public class GraphModel<TNode, TInput, TOutput> : GraphModel<TNode, TInput, TOutput, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TNode : GraphModel<TNode, TInput, TOutput, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TInput : INodeInputsModel, new()
        where TOutput : INodeOutputsModel, new()
    {
    }

    public class GraphModel<TNode, TInput, TOutput, TProperties> : GraphModel<TNode, TInput, TOutput, TProperties, GraphNodeBasicLayout>
        where TNode : GraphModel<TNode, TInput, TOutput, TProperties, GraphNodeBasicLayout>
        where TInput : INodeInputsModel, new()
        where TOutput : INodeOutputsModel, new()
        where TProperties : INodePropertiesModel, new()
    {
    }

    public class GraphModel<TNode, TInput, TOutput, TProperties, TLayout> : GraphNodeModel<TNode, TInput, TOutput, TProperties, TLayout>, IGraphModel
        where TNode : GraphNodeModel<TNode, TInput, TOutput, TProperties, TLayout>
        where TInput : INodeInputsModel, new()
        where TOutput : INodeOutputsModel, new()
        where TProperties : INodePropertiesModel, new()
        where TLayout : INodeLayoutModel, new()
    {
        public NodeModelsList Nodes { get; set; } = new();
        public LinkModelsList Links { get; set; } = new();
    }
}
