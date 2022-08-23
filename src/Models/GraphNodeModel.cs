using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Layout;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;
using Teriyaki.GraphFlow.Runtime;
using GraphNodeBasicOutputs = Teriyaki.GraphFlow.Models.Outputs.GraphNodeBasicOutputs;

namespace Teriyaki.GraphFlow
{
    public interface IGraphNodeModel
    {
        string Id { get; set; }

        [JsonExtensionData]
        JObject AdditionalData { get; set; }
    }
    public class GraphNodeModel<TNode> : GraphNodeModel<TNode, GraphNodeBasicInputs, GraphNodeBasicOutputs, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TNode : GraphNodeModel<TNode, GraphNodeBasicInputs, GraphNodeBasicOutputs, GraphNodeBasicProperties, GraphNodeBasicLayout>
    {
    }
    public class GraphNodeModel<TNode, TInput> : GraphNodeModel<TNode, TInput, GraphNodeBasicOutputs, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TNode : GraphNodeModel<TNode, TInput, GraphNodeBasicOutputs, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TInput : INodeInputsModel, new()
    {
    }

    public class GraphNodeModel<TNode, TInput, TOutput> : GraphNodeModel<TNode, TInput, TOutput, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TNode : GraphNodeModel<TNode, TInput, TOutput, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TInput : INodeInputsModel, new()
        where TOutput : INodeOutputsModel, new()
    {
    }

    public class GraphNodeModel<TNode, TInput, TOutput, TProperties> : GraphNodeModel<TNode, TInput, TOutput, TProperties, GraphNodeBasicLayout>
        where TNode : GraphNodeModel<TNode, TInput, TOutput, TProperties, GraphNodeBasicLayout>
        where TInput : INodeInputsModel, new()
        where TOutput : INodeOutputsModel, new()
        where TProperties : INodePropertiesModel, new()
    {
    }

    public class GraphNodeModel<TNode, TInput, TOutput, TProperties, TLayout> :
        IGraphNodeModel,
        IGraphNodeWithInputs<TInput>,
        IGraphNodeWithOutputs<TOutput>,
        IGraphNodeWithProperties<TProperties>,
        IGraphNodeWithLayout<TLayout>
        where TNode : GraphNodeModel<TNode, TInput, TOutput, TProperties, TLayout>
        where TInput : INodeInputsModel, new()
        where TOutput : INodeOutputsModel, new()
        where TProperties : INodePropertiesModel, new()
        where TLayout : INodeLayoutModel, new()
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public TInput InputsData { get; set; } = new TInput();
        public TOutput OutputsData { get; set; } = new TOutput();
        public TProperties PropertiesData { get; set; } = new TProperties();
        public TLayout LayoutData { get; set; } = new TLayout();
        [JsonExtensionData]
        public JObject AdditionalData { get; set; } = new JObject();

        public abstract class HandlerBase : NodeHandler<TNode, TInput, TOutput, TProperties, TLayout>
        {
        }

        public abstract class InputsValidator : AbstractValidator<TInput>
        {
        }
        /*
        public abstract class NodeRuntime : NodeRuntimeBase, GraphNodeModel<TNode, TInput, TOutput, TProperties, TLayout>.IRuntimeContext
        {
            public INodeHandler<GraphNodeModel<TNode, TInput, TOutput, TProperties, TLayout>.IRuntimeContext> TypedHandler => Handler as INodeHandler<GraphNodeModel<TNode, TInput, TOutput, TProperties, TLayout>.IRuntimeContext>;

            public override async Task Execute()
            {
                //
                await TypedHandler.OnExecute(this);
            }

            public Task<TValue> GetInputValue<TValue>(Expression<Func<TInput, TValue>> slot)
            {
                throw new NotImplementedException();
            }

            public Task<TValue> GetPropertyValue<TValue>(Expression<Func<TProperties, TValue>> slot)
            {
                throw new NotImplementedException();
            }

            public Task SetOutputValue<TValue>(Expression<Func<TOutput, TValue>> slot, TValue value)
            {
                throw new NotImplementedException();
            }

            public Task SetUnboundValue<TValue>(Expression<Func<TNode, TValue>> property, TValue value)
            {
                throw new NotImplementedException();
            }

            public Task<TValue> GetUnboundValue<TValue>(Expression<Func<TNode, TValue>> property)
            {
                throw new NotImplementedException();
            }

            public Task TriggerOutputSlot<TValue>(Expression<Func<TOutput, TValue>> slot)
            {
                throw new NotImplementedException();
            }
        }

        public interface IRuntimeContext
        {
            Task<TValue> GetInputValue<TValue>(Expression<Func<TInput, TValue>> slot);
            Task<TValue> GetPropertyValue<TValue>(Expression<Func<TProperties, TValue>> slot);
            Task SetOutputValue<TValue>(Expression<Func<TOutput, TValue>> slot, TValue value);
            Task SetUnboundValue<TValue>(Expression<Func<TNode, TValue>> property, TValue value);
            Task<TValue> GetUnboundValue<TValue>(Expression<Func<TNode, TValue>> property);
            Task TriggerOutputSlot<TValue>(Expression<Func<TOutput, TValue>> slot);
        }*/
    }
    /*
    public interface IWithNodeRuntime<TRuntime>
        where TRuntime : NodeRuntimeBase
    {
    }
    */

/*
public class GraphNodeModelBase
{
    public string Id { get; set; }

    // ----- Layout 
    public GraphNodeLayout Layout { get; set; }
}
public abstract class GraphNodeModel : GraphNodeModelBase
{

}

public abstract class GraphNodeModel<TNodeOutput> : GraphNodeModelBase
{

}*/
}