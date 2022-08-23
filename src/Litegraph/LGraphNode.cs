using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Teriyaki.GraphFlow.Models.Graph.Litegraph
{
    public class LGraphNode
    {
        public int id { get; set; } = -1;

        public int order { get; set; } = -1;
        public string type { get; set; }
        public double[] pos { get; set; }
        public LSize size { get; set; }

        public int mode { get; set; }
        public int shape { get; set; }

        public LGraphNodeFlags flags { get; set; }
        public List<LSlot> inputs { get; set; } = new List<LSlot>();
        public List<LSlot> outputs { get; set; } = new List<LSlot>();

        public LGraphNodeProperties properties { get; set; }
        /*
        public GraphNodeModel ToGraphNodeModel()
        {
            throw new NotImplementedException();
        }*/
    }

    public class LSize
    {
        // "size":{"0":140,"1":26}

        [Newtonsoft.Json.JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
    public class LGraphNodeProperties
    {
        [Newtonsoft.Json.JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

    public class LGraphNodeFlags
    {
        public bool collapsed { get; set; }
        public bool pinned { get; set; }
        public bool horizontal { get; set; }
        public bool render_box { get; set; }
    }
}
