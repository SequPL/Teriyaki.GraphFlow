using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Layout;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;

namespace Teriyaki.GraphFlow
{
    public interface INodeHandler 
    {
        Task OnExecute(IRuntimeContext c);
    }

    // For DI
    public interface INodeHandler<TNode> : INodeHandler
    {

    }
    public class NodeHandler<TNode> : NodeHandler<TNode, GraphNodeBasicInputs, GraphNodeBasicOutputs, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TNode : IGraphNodeModel, IGraphNodeWithInputs<GraphNodeBasicInputs>, IGraphNodeWithOutputs<GraphNodeBasicOutputs>, IGraphNodeWithProperties<GraphNodeBasicProperties>, IGraphNodeWithLayout<GraphNodeBasicLayout>
    {
        

    }
    public class NodeHandler<TNode, TInput> : NodeHandler<TNode, TInput, GraphNodeBasicOutputs, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TNode : IGraphNodeModel, IGraphNodeWithInputs<TInput>, IGraphNodeWithOutputs<GraphNodeBasicOutputs>, IGraphNodeWithProperties<GraphNodeBasicProperties>, IGraphNodeWithLayout<GraphNodeBasicLayout>
        where TInput : INodeInputsModel, new()
    {

    }
    public class NodeHandler<TNode, TInput, TOutput> : NodeHandler<TNode, TInput, TOutput, GraphNodeBasicProperties, GraphNodeBasicLayout>
        where TNode : IGraphNodeModel, IGraphNodeWithInputs<TInput>, IGraphNodeWithOutputs<TOutput>, IGraphNodeWithProperties<GraphNodeBasicProperties>, IGraphNodeWithLayout<GraphNodeBasicLayout>
        where TInput : INodeInputsModel, new()
        where TOutput : INodeOutputsModel, new()
    {

    }
    public class NodeHandler<TNode, TInput, TOutput, TProperties> : NodeHandler<TNode, TInput, TOutput, TProperties, GraphNodeBasicLayout>
        where TNode : IGraphNodeModel, IGraphNodeWithInputs<TInput>, IGraphNodeWithOutputs<TOutput>, IGraphNodeWithProperties<TProperties>, IGraphNodeWithLayout<GraphNodeBasicLayout>
        where TInput : INodeInputsModel, new()
        where TOutput : INodeOutputsModel, new()
        where TProperties : INodePropertiesModel, new()
    {

    }

    public class NodeHandler<TNode, TInput, TOutput, TProperties, TLayout> : INodeHandler<TNode>
        where TNode : IGraphNodeModel, IGraphNodeWithInputs<TInput>, IGraphNodeWithOutputs<TOutput>, IGraphNodeWithProperties<TProperties>, IGraphNodeWithLayout<TLayout>
        where TInput : INodeInputsModel, new()
        where TOutput : INodeOutputsModel, new()
        where TProperties : INodePropertiesModel, new()
        where TLayout : INodeLayoutModel, new()
    {
        public virtual Task OnExecute(IRuntimeContext c)
        {
            return Task.CompletedTask;
        }

        public virtual Task SetUnboundValue<TValue>(IRuntimeContext c, Expression<Func<TNode, TValue>> property, TValue value)
        {
            return c.SetUnboundValue(property, value);
        }
        public virtual Task<TValue> GetUnboundValue<TValue>(IRuntimeContext c, Expression<Func<TNode, TValue>> property)
        {
            return c.GetUnboundValue(property);
        }
        public virtual Task<TValue> GetInputValue<TValue>(IRuntimeContext c, Expression<Func<TInput, TValue>> slot)

        {
            return c.GetInputValue(slot);
        }
        public virtual Task<TValue> GetPropertyValue<TValue>(IRuntimeContext c, Expression<Func<TProperties, TValue>> property)
        {
            return c.GetPropertyValue(property);
        }
        public virtual Task SetPropertyValue<TValue>(IRuntimeContext c, Expression<Func<TProperties, TValue>> property, TValue value)
        {
            return c.SetPropertyValue(property, value);
        }
        public virtual Task SetOutputValue<TValue>(IRuntimeContext c, Expression<Func<TOutput, TValue>> slot, TValue value)
        {
            return c.SetOutputValue(slot, value);
        }
        public virtual async Task SetOutputFromProperty<TValue>(IRuntimeContext c, Expression<Func<TOutput, TValue>> slot, Expression<Func<TProperties, TValue>> property)
        {
            var value = await GetPropertyValue(c, property);
            await c.SetOutputValue(slot, value);
        }
        public virtual async Task SetOutputFromInput<TValue>(IRuntimeContext c, Expression<Func<TOutput, TValue>> slot, Expression<Func<TInput, TValue>> inputSlot)
        {
            var value = await GetInputValue(c, inputSlot);
            await c.SetOutputValue(slot, value);
        }
        public virtual async Task SetOutputFromUnbound<TValue>(IRuntimeContext c, Expression<Func<TOutput, TValue>> slot, Expression<Func<TNode, TValue>> property)
        {
            var value = await GetUnboundValue(c, property);
            await c.SetOutputValue(slot, value);
        }
        public virtual Task TriggerOutputSlot<TValue>(IRuntimeContext c, Expression<Func<TOutput, TValue>> slot)
        {
            return c.TriggerOutputSlot(slot);
        }

        public virtual Task<DynamicInput[]> GetDynamicInputs(IRuntimeContext c)
        {
            return c.GetDynamicInputs();
        }
    }
}
