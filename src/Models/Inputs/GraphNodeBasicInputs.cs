using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Teriyaki.GraphFlow.Models.Inputs
{
    public class GraphNodeBasicInputs : INodeInputsModel
    {
        [JsonExtensionData]
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IDictionary<string, JToken> AdditionalData { get; set; } = new ConcurrentDictionary<string, JToken>();

        [NodeInput(DefaultValue = true)] public bool Enable { get; set; }

        public DynamicInputsSet DynamicInputs { get; set; } = new ();
    }

    
}