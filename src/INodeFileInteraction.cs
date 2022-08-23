using System.Threading.Tasks;

namespace Teriyaki.GraphFlow
{
    public interface INodeFileInteraction
    {
        // TODO: typ file <- na razie jest placeholder
        Task onDropFile(IRuntimeContext c, object file);
    }
}