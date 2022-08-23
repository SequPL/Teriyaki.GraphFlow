using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Runtime;

namespace Teriyaki.GraphFlow
{
    public interface IRuntimeContext
    {
        Task SendCommand(IRequest command);

        Task TriggerOutputSlot(OutputRuntime slot);
        //
        //Task SetValue<TData, TValue>(TData data, Expression<Func<TData, TValue>> property, TValue value);
        //Task<TValue> GetValue<TData, TValue>(TData data, Expression<Func<TData, TValue>> property);

        Task<object> GetValue<TData>(TData data, PropertyInfo property);
        Task SetValue<TData>(TData data, PropertyInfo property, object value);

        Task<OutputRuntime> GetOutput(string name);
        Task<InputRuntime> GetInput(string name);
        Task<PropertyRuntime> GetProperty(string name);
        //
        IGraphNodeModel Node { get; }
    }
}