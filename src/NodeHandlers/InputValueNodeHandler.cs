using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Models.Layout;
using Teriyaki.GraphFlow.Models.Outputs;
using Teriyaki.GraphFlow.Models.Properties;
using Teriyaki.GraphFlow.Nodes;

namespace Teriyaki.GraphFlow.NodeHandlers
{
    public class InputValueNodeHandler : InputValueNode.HandlerBase
    {
        public override async Task OnExecute(IRuntimeContext c)
        {
            await SetOutputFromInput(c, q => q.Value, q => q.Value);
            await SetPropertyValue(c, q => q.IsExecuted, true);
            //
            await base.OnExecute(c);
        }
        /*
        public override Task OnExecute(IRuntimeContext c)
        {
            var x = GetInputValue(c, q => q.Value);
            //
            return Task.CompletedTask;
        }*/
    }
}
