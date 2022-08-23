using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Nodes;

namespace Teriyaki.GraphFlow.NodeHandlers
{
    public class ButtonNodeHandler : ButtonNode.HandlerBase, INodeMouseInteraction
    {
        public override async Task OnExecute(IRuntimeContext c)
        {
            await SetOutputFromUnbound(c, q => q.IsClicked, q => q.IsClicked);
            //
            await base.OnExecute(c);
        }

        public async Task OnMouseDown(IRuntimeContext c)
        {
            await SetUnboundValue(c, q => q.IsClicked, true);
            await SetOutputValue(c, q => q.IsClicked, true);
            //
            await TriggerOutputSlot(c, q => q.IsClicked);
        }

        public async Task OnMouseUp(IRuntimeContext c)
        {
            await SetUnboundValue(c, q => q.IsClicked, false);
            await SetOutputValue(c, q => q.IsClicked, false);
            //
            await TriggerOutputSlot(c, q => q.IsClicked);
        }
    }
}