using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;

namespace Teriyaki.GraphFlow
{
    public static class IRuntimeContextExtensions
    {
        public static async Task<object> GetInputValue<TInputs>(this IRuntimeContext @this, Expression<Func<TInputs, object>> slot)
            where TInputs : INodeInputsModel, new()
        {
            return await GetInputValue<TInputs, object>(@this, slot);
        }

        public static async Task<TValue> GetInputValue<TInputs, TValue>(this IRuntimeContext @this, Expression<Func<TInputs, TValue>> slot)
            where TInputs : INodeInputsModel, new()
        {
            if (@this.Node.GetInputsType() == null) throw new AccessViolationException($"Node {@this.Node} isn't {typeof(IGraphNodeWithInputs<>).Name}");
            var p = slot.GetMemberPropertyInfo();
            var input = await @this.GetInput(p.Name);
            //
            return (TValue)input.GetValue();
        }
        public static async Task<object> GetInputValue(this IRuntimeContext @this, string slotName)
        {
            var input = await @this.GetInput(slotName);
            //
            return input.GetValue();
        }

        public static async Task<TValue> GetPropertyValue<TProperties, TValue>(this IRuntimeContext @this, Expression<Func<TProperties, TValue>> slot)
            where TProperties : INodePropertiesModel, new()
        {
            if (@this.Node.GetPropertiesType() == null) throw new AccessViolationException($"Node {@this.Node} isn't {typeof(IGraphNodeWithProperties<>).Name}");
            //
            var p = slot.GetMemberPropertyInfo();
            var property = await @this.GetProperty(p.Name);
            //
            return (TValue)property.GetValue();
        }
        public static async Task SetPropertyValue<TProperties, TValue>(this IRuntimeContext @this, Expression<Func<TProperties, TValue>> slot, TValue value)
            where TProperties : INodePropertiesModel, new()
        {
            if (@this.Node.GetPropertiesType() == null) throw new AccessViolationException($"Node {@this.Node} isn't {typeof(IGraphNodeWithProperties<>).Name}");
            //
            var p = slot.GetMemberPropertyInfo();
            await SetPropertyValue(@this, p.Name, value);
        }
        public static async Task SetPropertyValue<TValue>(this IRuntimeContext @this, string slot, TValue value)
        {
            if (@this.Node.GetPropertiesType() == null) throw new AccessViolationException($"Node {@this.Node} isn't {typeof(IGraphNodeWithProperties<>).Name}");
            //
            var property = await @this.GetProperty(slot);
            //
            property.SetValue(value);
        }
        public static async Task SetOutputValue<TOutput, TValue>(this IRuntimeContext @this, Expression<Func<TOutput, TValue>> slot, TValue value)
            where TOutput : INodeOutputsModel, new()
        {
            if (@this.Node.GetOutputsType() == null) throw new AccessViolationException($"Node {@this.Node} isn't {typeof(IGraphNodeWithOutputs<>).Name}");
            var p = slot.GetMemberPropertyInfo();
            //
            var accessor = await @this.GetOutput(p.Name);
            accessor.SetValue(value);
        }
        public static async Task TriggerOutputSlot<TOutput, TValue>(this IRuntimeContext @this, Expression<Func<TOutput, TValue>> slot) where TOutput : INodeOutputsModel, new()
        {
            var p = slot.GetMemberPropertyInfo();
            var accessor = await @this.GetOutput(p.Name);
            //
            await @this.TriggerOutputSlot(accessor);
        }
        /*
        public Task SetValue<TData, TValue>(this IRuntimeContext @this, TData data, Expression<Func<TData, TValue>> property, TValue value)
        {
            var p = property.GetMemberPropertyInfo();
            //
            if (p.DeclaringType != data.GetType()) throw new AccessViolationException();
            //
            SetValue(data, p, value);
            //
            return Task.CompletedTask;
        }*/

        public static async Task SetUnboundValue<TNodeRoot, TValue>(this IRuntimeContext @this, Expression<Func<TNodeRoot, TValue>> property, TValue value)
            where TNodeRoot : IGraphNodeModel
        {
            var p = property.GetMemberPropertyInfo();
            var propertyInfo = @this.Node.GetType().GetProperty(p.Name);
            //
            await @this.SetValue((TNodeRoot) @this.Node, propertyInfo, value);
        }

        public static async Task<TValue> GetUnboundValue<TNodeRoot, TValue>(this IRuntimeContext @this, Expression<Func<TNodeRoot, TValue>> property)
            where TNodeRoot : IGraphNodeModel
        {
            var p = property.GetMemberPropertyInfo();
            var propertyInfo = @this.Node.GetType().GetProperty(p.Name);
            //
            return (TValue)(await @this.GetValue((TNodeRoot) @this.Node, propertyInfo));
        }

        public static Task<DynamicInput[]> GetDynamicInputs(this IRuntimeContext @this)
        {
            var data = @this.Node.GetInputsData() as GraphNodeBasicInputs;
            //
            return Task.FromResult(data.DynamicInputs?.ToArray() ?? new DynamicInput[]{});
            /*
            var data = @this.Node.GetInputsData();
            //
            return Task.FromResult(data.AdditionalData.Select(q => new DynamicInput()
            {
                Name = q.Key,
                Type = q.Value.Type,
                Value = q.Value == null ? GetDefaultValue(q.Value.Type) : q.Value.ToObject<object>(),
                InputAttribute = new NodeInputAttribute()
                {
                    DefaultValue = GetDefaultValue(q.Value.Type)
                }
            }).ToArray());*/
        }

        public static object GetDefaultValue(JTokenType type)
        {
            switch (type)
            {
                case JTokenType.Date:
                    return DateTime.MinValue;
                case JTokenType.Boolean:
                    return false;
                case JTokenType.Float:
                case JTokenType.Integer:
                    return (long) 0;
                //
                default:
                    return null;
            }
        }
    }
}