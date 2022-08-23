using System.Threading.Tasks;

namespace Teriyaki.GraphFlow
{
    public interface INodeMouseInteraction
    {
        Task OnMouseDown(IRuntimeContext c);
        Task OnMouseUp(IRuntimeContext c);
    }
}