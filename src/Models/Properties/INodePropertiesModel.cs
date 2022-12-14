using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Teriyaki.GraphFlow.Models.Properties
{
    public interface INodePropertiesModel
    {
        [JsonExtensionData]
        IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
