using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teriyaki.GraphFlow;

namespace Teriyaki
{
    public static class GraphModelExtensions
    {
        public static TNode AddNode<TNode>(this GraphModel @this, TNode node)
            where TNode : IGraphNodeModel
        {
            @this.Nodes.Add(node);
            //
            return node;
        }

        public static GraphLinkModel AddLink(this GraphModel @this, GraphLinkModel link)
        {
            @this.Links.Add(link);
            //
            return link;
        }
    }
}
