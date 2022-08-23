namespace Teriyaki.GraphFlow
{
    public interface INodesFactory
    {
        INodeRuntime CreateNodeRuntime(IGraphNodeModel node);
    }
}