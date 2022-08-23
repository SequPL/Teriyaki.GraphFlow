using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teriyaki.GraphFlow.Runtime;

namespace Teriyaki.GraphFlow
{
    public static class GraphRuntimeExtensions
    {
        public static NodeRuntime<TNode>? GetNodeRuntime<TNode>(this GraphRuntime @this, TNode node)
            where TNode : class, IGraphNodeModel
        {
            return @this.GetNodeRuntime<TNode>(node.Id);
        }
    }
}
