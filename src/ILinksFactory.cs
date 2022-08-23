using Teriyaki.GraphFlow.Runtime;

namespace Teriyaki.GraphFlow
{
    public interface ILinksFactory
    {
        ILinkRuntime CreateLinkRuntime(GraphLinkModel link);
    }
}