using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Teriyaki.GraphFlow.Models.Inputs;

namespace Teriyaki.GraphFlow
{
    public static class IGraphNodeWithInputsExtensions
    {
        public static Task<TValue> GetInputValue<TInput, TValue>(this IGraphNodeWithInputs<TInput> @this, IRuntimeContext runtime, Expression<Func<TInput, TValue>> slot)
            where TInput : INodeInputsModel, new()
        {
            return runtime.GetInputValue(slot);
        }
    }
}
