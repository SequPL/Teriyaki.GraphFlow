using System;
using System.Collections.Generic;
using System.Text;

namespace Teriyaki.GraphFlow.Models.Graph.Litegraph
{
    public class LGraph
    {
        public List<LGraphNode> nodes { get; set; } = new List<LGraphNode>();
        public List<List<object>> links { get; set; } = new List<List<object>>(); // mogą być inty i stringi

        public List<LGroup> groups { get; set; } = new List<LGroup>();
        //
        public Dictionary<int, LGraphNode> nodes_by_id { get; set; } = new Dictionary<int, LGraphNode>();
        public List<LGraphNode> nodes_in_execution_order { get; set; } = new List<LGraphNode>();

        public bool has_errors { get; set; } = false;
        public int last_node_id { get; set; } = 0;
        public int last_link_id { get; set; } = 0;
        public double time { get; set; } = 0; //time in seconds

        public Dictionary<string, float> outputs { get; set; } = new Dictionary<string, float>();
    }
}
