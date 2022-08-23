using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Teriyaki.GraphFlow
{
    [JsonArray(ItemTypeNameHandling = TypeNameHandling.Auto)]
    public class NodeModelsList : List<IGraphNodeModel>
    {
        public TNode GetNode<TNode>(string nodeId)
        {
            return (TNode)this.FirstOrDefault(q => q.Id == nodeId);
        }
    }
}
