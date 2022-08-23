using System;
using System.Collections.Generic;
using System.Text;

namespace Teriyaki.GraphFlow.Models.Graph.Litegraph
{
    public class LSlot
    {
        public int num { get; set; } // nie wiem
        public string name { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public List<int> links { get; set; } = new List<int>(); //for output slots
    }
}
