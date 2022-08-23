using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teriyaki.GraphFlow.Models.Layout
{
    public interface IGraphNodeWithLayout<TLayout> : IGraphNodeModel
        where TLayout : INodeLayoutModel, new()
    {
        TLayout LayoutData { get; set; }
    }
}
