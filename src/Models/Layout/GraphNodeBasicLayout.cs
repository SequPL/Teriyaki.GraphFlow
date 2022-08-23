using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Teriyaki.GraphFlow.Models.Layout
{
    public class GraphNodeBasicLayout : INodeLayoutModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Size Size { get; set; }

        public Vector2 Position { get; set; }
        //
        public string Shape { get; set; }

        public bool Resizable { get; set; }

        // if the slots should be placed horizontally on the top and bottom of the node
        public bool Horizontal { get; set; }
        public string Tooltip { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
