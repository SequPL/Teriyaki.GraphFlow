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
    public interface INodeLayoutModel
    {
        string Title { get; set; }
        string Description { get; set; }
        Size Size { get; set; }
        Vector2 Position { get; set; }
        //
        string Shape { get; set; }

        bool Resizable { get; set; }

        // if the slots should be placed horizontally on the top and bottom of the node
        bool Horizontal { get; set; }
        string Tooltip { get; set; }

        [JsonExtensionData]
        IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
