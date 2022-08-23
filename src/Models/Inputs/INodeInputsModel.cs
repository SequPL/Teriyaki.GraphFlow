using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Teriyaki.GraphFlow.Nodes;

namespace Teriyaki.GraphFlow.Models.Inputs
{
    public interface INodeInputsModel 
    {
        [JsonExtensionData]
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IDictionary<string, JToken> AdditionalData { get; set; }

        DynamicInputsSet DynamicInputs { get; set; }

        [NodeInput(DefaultValue = true)]
        bool Enable { get; set; }
    }
}
