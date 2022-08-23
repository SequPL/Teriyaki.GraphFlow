using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Nodes;

namespace Teriyaki.GraphFlow.NodeHandlers
{
    public class ConstStringNodeHandler : ConstStringNode.HandlerBase, INodeHandlerInputValidator<ConstStringNodeHandler.InputsDataValidator>
    {
        public class InputsDataValidator : ConstStringNode.InputsValidator
        {
            public InputsDataValidator()
            {
                
            }
        }

        public override async Task OnExecute(IRuntimeContext c)
        {
            await SetOutputFromProperty(c, q => q.Value, q => q.Value);
            //
            await base.OnExecute(c);
        }
    }
}
