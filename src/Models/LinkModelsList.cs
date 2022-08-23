using System.Collections.Generic;
using Newtonsoft.Json;

namespace Teriyaki.GraphFlow
{
    [JsonArray(ItemTypeNameHandling = TypeNameHandling.Auto)]
    public class LinkModelsList : List<GraphLinkModel>
    {
    }
}
