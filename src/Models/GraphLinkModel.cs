using System;
using System.Collections.Generic;
using System.Text;

namespace Teriyaki.GraphFlow
{
    public class GraphLinkModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string InputId { get; set; }
        public string OutputId { get; set; }
    }
}
