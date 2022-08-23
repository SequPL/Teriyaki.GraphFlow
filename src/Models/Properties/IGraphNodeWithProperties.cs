using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teriyaki.GraphFlow.Models.Properties
{
    public interface IGraphNodeWithProperties<TProperties> : IGraphNodeModel
        where TProperties : INodePropertiesModel, new()
    {
        TProperties PropertiesData { get; set; }
    }
}
