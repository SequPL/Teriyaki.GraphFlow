using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Newtonsoft.Json.Linq;
using Serilog;
using Teriyaki.GraphFlow.Comparers;
using Teriyaki.GraphFlow.Models.Graph.Attributes;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;
using Teriyaki.GraphFlow.Reflections;

namespace Teriyaki.GraphFlow.Runtime
{
    public class NodeRuntime<TNode> : INodeRuntime, IRuntimeContext
        where TNode : IGraphNodeModel
    {
        private GraphRuntime _graphRuntime;
        private PropertyInfo _enableProperty;

        public uint LinksToStart { get; set; }
        public string Id => Node.Id;
        public bool Enable => (bool) PropertyInfoAccessors.GetValue(Node.GetInputsData(), _enableProperty);
  

        public IGraphNodeModel Node { get; protected set; }

        public INodeHandler Handler { get; protected set; }

        public IValidator InputValidator { get; protected set; }

        //
        public IReadOnlySet<InputRuntime> Inputs { get; protected set; }

        public IReadOnlySet<OutputRuntime> Outputs { get; protected set; }
        public IReadOnlySet<PropertyRuntime> Properties { get; protected set; }
        public NodeRuntime()
        {
        }

        public virtual async Task Execute()
        {
            foreach (var output in Outputs) output.SetValue(output.OutputAttribute?.DefaultValue);
            foreach (var input in Inputs.Where(q => !q.Links.Any())) input.SetValue(input.InputAttribute?.DefaultValue);
            //
            if (Enable) await Handler.OnExecute(this);
        }

        //
        public virtual Task Init(GraphRuntime graphRuntime, IGraphNodeModel node)
        {
            _graphRuntime = graphRuntime;
            Node = node;
            //
            Handler = graphRuntime.CreateNodeHandler(node);
            //InputValidator = CreateValidator();
            //
            Inputs = CreateInputs();
            Outputs = CreateOutputs();
            Properties = CreateProperties();
            //
            return Task.CompletedTask;
        }
        /*
        private IValidator CreateValidator()
        {
            var validatorType = Handler.GetInputValidatorType();
            if (validatorType == null) return null;
            //
            return Activator.CreateInstance(validatorType) as IValidator;
        }*/

        protected virtual IReadOnlySet<InputRuntime> CreateInputs()
        {
            IEnumerable<InputRuntime> inputs()
            {
                var props = Node.GetInputsType().GetProperties();
                //
                foreach (var p in props)
                {
                    if (p.Name == nameof(INodeInputsModel.AdditionalData)) continue;
                    if (p.Name == nameof(INodeInputsModel.DynamicInputs)) continue;
                    if (p.Name == nameof(INodeInputsModel.Enable)) _enableProperty = p;
                    //
                    var o = p.GetCustomAttribute<NodeInputAttribute>();
                    //
                    yield return new InputRuntime()
                    {
                        Node = this,
                        Name = p.Name,
                        InputAttribute = o,
                        //
                        GetValue = () => PropertyInfoAccessors.GetValue(Node.GetInputsData(), p),
                        SetValue = (value) =>
                        {
                            Log.Debug("Set value for input {id}, value : {value}", Node.GetInputId(p.Name), value);
                            //
                            if (value == null || p.PropertyType == value.GetType())
                            {
                                PropertyInfoAccessors.SetValue(Node.GetInputsData(), p, value);
                                return value;
                            }
                            //
                            Log.Debug("Value is another type as property, {valueType} {propertyType} so need convert.", value.GetType().Name, p.PropertyType.Name);
                            //
                            var converter = TypeDescriptor.GetConverter(value.GetType());
                            Log.Debug("Converter used to convert from {valueType} to {propertyType} is : {converter}.", value.GetType().Name, p.PropertyType.Name, converter);
                            //
                            var convertedValue = converter.ConvertTo(value, p.PropertyType);
                            PropertyInfoAccessors.SetValue(Node.GetInputsData(), p, convertedValue);
                            //
                            return convertedValue;
                        },
                    };
                }
                //
                foreach (var input in this.GetDynamicInputs().Result)
                {
                    yield return new InputRuntime()
                    {
                        Node = this,
                        Name = input.Name,
                        InputAttribute = input.InputAttribute,
                        //
                        IsDynamic = true,
                        //
                        GetValue = () => Node.GetInputsData()?.DynamicInputs[input.Name]?.Value,
                        SetValue = (value) =>
                        {
                            Log.Debug("Set value for input {id}, value : {value}", Node.GetInputId(input.Name), value);
                            //
                            var data = Node.GetInputsData();
                            //
                            if (data.DynamicInputs.FirstOrDefault(q=> q.Name == input.Name) != null)
                                data.DynamicInputs[input.Name].Value = value;
                            else
                                data.DynamicInputs.Add(input.Name, new DynamicInput(value));
                            //
                            return value;
                        },
                    };
                }
            }
            //
            return inputs().ToHashSet(new IPropertyAccessorIdComparer<InputRuntime>());
        }
        protected virtual IReadOnlySet<OutputRuntime> CreateOutputs()
        {
            IEnumerable<OutputRuntime> outputs()
            {
                var props = Node.GetOutputsType().GetProperties();
                //
                foreach (var p in props)
                {
                    if (p.Name == nameof(INodeOutputsModel.AdditionalData)) continue;
                    //
                    var o = p.GetCustomAttribute<NodeOutputAttribute>();
                    //
                    yield return new OutputRuntime()
                    {
                        Node = this,
                        Name = p.Name,
                        OutputAttribute = o,
                        //
                        //
                        GetValue = () => PropertyInfoAccessors.GetValue(Node.GetOutputsData(), p),
                        SetValue = (value) =>
                        {
                            if (value == null || p.PropertyType == value.GetType())
                            {
                                PropertyInfoAccessors.SetValue(Node.GetOutputsData(), p, value);
                                return value;
                            }
                            //
                            var converter = TypeDescriptor.GetConverter(value.GetType());
                            //
                            var convertedValue = converter.ConvertTo(value, p.PropertyType);
                            PropertyInfoAccessors.SetValue(Node.GetOutputsData(), p, convertedValue);
                            //
                            return convertedValue;
                        },
                    };
                }
            }

            //
            return outputs().ToHashSet(new IPropertyAccessorIdComparer<OutputRuntime>());
        }
        protected virtual IReadOnlySet<PropertyRuntime> CreateProperties()
        {
            IEnumerable<PropertyRuntime> properties()
            {
                var props = Node.GetPropertiesType().GetProperties();
                //
                foreach (var p in props)
                {
                    if (p.Name == nameof(INodePropertiesModel.AdditionalData)) continue;
                    //
                    var o = p.GetCustomAttribute<NodePropertyAttribute>();
                    //
                    yield return new PropertyRuntime()
                    {
                        Node = this,
                        Name = p.Name,
                        PropertyAttribute = o,
                        //
                        GetValue = () => PropertyInfoAccessors.GetValue(Node.GetPropertiesData(), p),
                        SetValue = (value) =>
                        {
                            if (value == null || p.PropertyType == value.GetType())
                            {
                                PropertyInfoAccessors.SetValue(Node.GetPropertiesData(), p, value);
                                return value;
                            }
                            //
                            var converter = TypeDescriptor.GetConverter(value.GetType());
                            //
                            var convertedValue = converter.ConvertTo(value, p.PropertyType);
                            PropertyInfoAccessors.SetValue(Node.GetPropertiesData(), p, convertedValue);
                            //
                            return convertedValue;
                        },
                    };
                }
            }

            //
            return properties().ToHashSet(new IPropertyAccessorIdComparer<PropertyRuntime>());
        }
        
        //public IEnumerable<PropertyRuntime> Properties { get; set; }

        public Task SendCommand(IRequest command)
        {
            throw new NotImplementedException();
        }

        public Task TriggerOutputSlot(OutputRuntime slot) 
        {
            return _graphRuntime.TriggerOutputSlot(slot);
        }

        public Task<object> GetValue<TData>(TData data, PropertyInfo property)
        {
            return Task.FromResult(PropertyInfoAccessors.GetValue(data, property));
        }

        public Task SetValue<TData>(TData data, PropertyInfo property, object value)
        {
            PropertyInfoAccessors.SetValue(data, property, value);
            //
            return Task.CompletedTask;
        }

        
        public Task<OutputRuntime> GetOutput(string name)
        {
            return Task.FromResult(Outputs.FirstOrDefault(q => q.Name == name));
        }
        public Task<InputRuntime> GetInput(string name)
        {
            return Task.FromResult(Inputs.FirstOrDefault(q => q.Name == name));
        }
        public Task<PropertyRuntime> GetProperty(string name)
        {
            return Task.FromResult(Properties.FirstOrDefault(q => q.Name == name));
        }



        /*
        public async Task<TValue> GetValue<TData, TValue>(TData data, Expression<Func<TData, TValue>> property)
        {
            var p = property.GetMemberPropertyInfo();
            var value = await GetValue(data, p);
            //
            return (TValue) value!;
        }*/
        public virtual ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }

    /*public class NodeRuntime<TNode> : NodeRuntime, IRuntimeContext
        where TNode : class, IGraphNodeModel
    {
        public IValidator<TNode> TypedValidator => Validator as IValidator<TNode>;

        public TNode TypedNode => Node as TNode;
        //
        public override async Task Execute()
        {
            if (Validator != null)
            {
                var validationResult = await TypedValidator?.ValidateAsync(TypedNode)!;
                if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
            }
            //
            await Handler.OnExecute(this);
        }

        public Task<TValue> GetInputValue<TInput, TValue>(Expression<Func<TInput, TValue>> slot) where TInput : INodeInputsModel, new()
        {
            throw new NotImplementedException();
        }

        public Task<TValue> GetPropertyValue<TProperties, TValue>(Expression<Func<TProperties, TValue>> slot) where TProperties : INodePropertiesModel, new()
        {
            throw new NotImplementedException();
        }

        public Task SetOutputValue<TOutput, TValue>(Expression<Func<TOutput, TValue>> slot, TValue value) where TOutput : INodeOutputsModel, new()
        {
            throw new NotImplementedException();
        }

        public Task SetUnboundValue<TNode1, TValue>(Expression<Func<TNode1, TValue>> property, TValue value) where TNode1 : IGraphNodeModel
        {
            throw new NotImplementedException();
        }

        public Task<TValue> GetUnboundValue<TNode1, TValue>(Expression<Func<TNode1, TValue>> property) where TNode1 : IGraphNodeModel
        {
            throw new NotImplementedException();
        }

        public Task TriggerOutputSlot<TOutput, TValue>(Expression<Func<TOutput, TValue>> slot) where TOutput : INodeOutputsModel, new()
        {
            throw new NotImplementedException();
        }
    }*/
    /*
    public class NodeRuntime<TNode> : NodeRuntime, INodeRuntime<TNode>
        where TNode : GraphNodeModelBase
    {
        public IValidator<TNode> TypedValidator => Validator as IValidator<TNode>;
        public TNode TypedNode => Node as TNode;


        public INodeHandler<TNode> TypedHandler => Handler as INodeHandler<TNode>;
        //

        //
        public override async Task Execute()
        {
            if (Validator != null)
            {
                var validationResult = await TypedValidator?.ValidateAsync(TypedNode)!;
                if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
            }
            //
            await TypedHandler.OnExecute(TypedNode, this);
        }

        public Task<TSlotValue> GetInputValue<TSlotValue>(Expression<Func<TNode, TSlotValue>> slot)
        {
            throw new NotImplementedException();
        }

        public Task SendCommand(IRequest command)
        {
            throw new NotImplementedException();
        }
    }

    public class NodeRuntime<TNode, TNodeOutput> : NodeRuntime, INodeRuntime<TNode, TNodeOutput>
        where TNode : GraphNodeModel<TNodeOutput>
    {
        public TNode TypedNode => Node as TNode;
        public INodeHandler<TNode, TNodeOutput> TypedHandler => Handler as INodeHandler<TNode, TNodeOutput>;
        public IValidator<TNode> TypedValidator => Validator as IValidator<TNode>;

        public override async Task Execute()
        {
            if (Validator != null)
            {
                var validationResult = await TypedValidator?.ValidateAsync(TypedNode)!;
                if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
            }
            //
            await TypedHandler.OnExecute(TypedNode, this);
        }

        public Task SendCommand(IRequest command)
        {
            throw new NotImplementedException();
        }

        public Task SetOutputValue<TSlotValue>(Expression<Func<TNodeOutput, TSlotValue>> slot, TSlotValue value)
        {
            throw new NotImplementedException();
        }

        public Task<TSlotValue> GetInputValue<TSlotValue>(Expression<Func<TNodeOutput, TSlotValue>> slot)
        {
            throw new NotImplementedException();
        }

        public Task TriggerSlot<TSlotValue>(Expression<Func<TNodeOutput, TSlotValue>> slot)
        {
            throw new NotImplementedException();
        }
    }
    */
}