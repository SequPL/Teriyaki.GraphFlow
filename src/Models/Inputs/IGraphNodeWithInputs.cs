using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teriyaki.GraphFlow.Models.Inputs
{
    public interface IGraphNodeWithInputs<TInputs> : IGraphNodeModel
        where TInputs : INodeInputsModel, new()
    {
        TInputs InputsData { get; set; }
    }
}
