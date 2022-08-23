using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teriyaki.GraphFlow.Models.Outputs
{
    public interface IGraphNodeWithOutputs<TOutputs> : IGraphNodeModel
        where TOutputs : INodeOutputsModel, new()
    {
        TOutputs OutputsData { get; set; }
    }
}
