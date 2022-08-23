using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicExpresso;
using Teriyaki.GraphFlow.Nodes;

namespace Teriyaki.GraphFlow.NodeHandlers
{
    public class IfNodeHandler : IfNode.HandlerBase
    {
        public override async Task OnExecute(IRuntimeContext c)
        {
            var interpreter = new Interpreter(InterpreterOptions.DefaultCaseInsensitive);
            //
            var inputs = await GetDynamicInputs(c);
            var condition = await GetPropertyValue(c, q => q.Condition);
            //
            foreach (var input in inputs) interpreter.SetVariable(input.Name, Convert.ChangeType(input.Value, input.Type), input.Type);
            //
            var result = interpreter.Eval<bool>(condition.Value);
            //
            await SetOutputValue(c, q => q.IsTrue, result);
            await SetOutputValue(c, q => q.IsFalse, !result);
            //
            await base.OnExecute(c);
        }
    }
}