using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Internal;
using Newtonsoft.Json.Linq;
using Teriyaki.GraphFlow;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;
using Teriyaki.GraphFlow.Nodes;
using Teriyaki.GraphFlow.Reflections;
using Teriyaki.GraphFlow.Runtime;

namespace Teriyaki
{
    public static class GraphNodeModelBaseExtensions
    {
        public static string GetOutputId<TNodeOutput, TSlotValue>(this IGraphNodeWithOutputs<TNodeOutput> @this, Expression<Func<TNodeOutput, TSlotValue>> slot)
            where TNodeOutput : INodeOutputsModel, new()
        {
            var property = slot.GetMemberPropertyInfo();
            return GetOutputId(@this, property);
        }

        public static string GetOutputId(this IGraphNodeModel @this, PropertyInfo slotProperty)
        {
            return GetOutputId(@this, slotProperty.Name);
        }

        public static string GetOutputId(this IGraphNodeModel @this, string slotPropertyName)
        {
            return slotPropertyName == null ? null : "out-" + @this.Id + "-" + slotPropertyName;
        }

        public static string GetInputId<TNodeInput, TSlotValue>(this IGraphNodeWithInputs<TNodeInput> @this, Expression<Func<TNodeInput, TSlotValue>> slot)
            where TNodeInput : INodeInputsModel, new()
        {
            var property = slot.GetMemberPropertyInfo();
            return GetInputId(@this, property);
        }

        public static string GetInputId(this IGraphNodeModel @this, PropertyInfo slotProperty)
        {
            return GetInputId(@this, slotProperty.Name);
        }

        public static string GetInputId(this IGraphNodeModel @this, string slotPropertyName)
        {
            return slotPropertyName == null ? null : "in-" + @this.Id + "-" + slotPropertyName;
        }

        public static string GetInputName(this IGraphNodeModel @this, string slotId)
        {
            return slotId.Split('-').Last();
        }
        public static string GetOutputName(this IGraphNodeModel @this, string slotId)
        {
            return slotId.Split('-').Last();
        }

        public static string GetPropertyId(this IGraphNodeModel @this, string propertyName)
        {
            return propertyName == null ? null : "p-" + @this.Id + "-" + propertyName;
        }

        public static Type? GetNodeRuntimeType(this IGraphNodeModel node)
        {
            return typeof(NodeRuntime<>).MakeGenericType(node.GetType());
        }

        public static Type? GetHandlerType(this IGraphNodeModel node)
        {
            return typeof(INodeHandler<>).MakeGenericType(node.GetType());
        }

        public static Type? GetOutputsType(this IGraphNodeModel node)
        {
            return node.GetType().GetGenericTypeFromInterface(typeof(IGraphNodeWithOutputs<>));
        }

        public static Type? GetPropertiesType(this IGraphNodeModel node)
        {
            return node.GetType().GetGenericTypeFromInterface(typeof(IGraphNodeWithProperties<>));
        }

        public static Type? GetInputsType(this IGraphNodeModel node)
        {
            return node.GetType().GetGenericTypeFromInterface(typeof(IGraphNodeWithInputs<>));
        }

        public static INodeInputsModel? GetInputsData(this IGraphNodeModel @this)
        {
            if (@this.GetInputsType() == null) throw new AccessViolationException($"Node {@this.ToString()} isn't {typeof(IGraphNodeWithInputs<>).Name}");
            //
            var property = @this.GetType().GetProperty(nameof(IGraphNodeWithInputs<GraphNodeBasicInputs>.InputsData));
            return PropertyInfoAccessors.GetValue(@this, property) as INodeInputsModel;
        }

        public static INodeOutputsModel? GetOutputsData(this IGraphNodeModel @this)
        {
            if (@this.GetOutputsType() == null) throw new AccessViolationException($"Node {@this.ToString()} isn't {typeof(IGraphNodeWithOutputs<>).Name}");
            //
            var property = @this.GetType().GetProperty(nameof(IGraphNodeWithOutputs<GraphNodeBasicOutputs>.OutputsData));
            return PropertyInfoAccessors.GetValue(@this, property) as INodeOutputsModel;
        }

        public static INodePropertiesModel? GetPropertiesData(this IGraphNodeModel @this)
        {
            if (@this.GetPropertiesType() == null) throw new AccessViolationException($"Node {@this.ToString()} isn't {typeof(IGraphNodeWithProperties<>).Name}");
            //
            var property = @this.GetType().GetProperty(nameof(IGraphNodeWithProperties<GraphNodeBasicProperties>.PropertiesData));
            return PropertyInfoAccessors.GetValue(@this, property) as INodePropertiesModel;
        }
        /*
        public static DynamicInput AddDynamicInput<TNodeInput>(this IGraphNodeWithInputs<TNodeInput> @this, string name, object value)
            where TNodeInput : INodeInputsModel, new()
        {
            var di = new DynamicInput()
            {
                Name = name,
                Value = value
            };
            //
            @this.GetInputsData()?.AdditionalData.Add(name, JToken.FromObject(value));
            //
            return di;
        }*/
    }
}