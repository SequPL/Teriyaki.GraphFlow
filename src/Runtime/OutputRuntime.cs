namespace Teriyaki.GraphFlow.Runtime
{
    public record OutputRuntime : LinkSlotRuntime
    {
        public override string Id => Node.Node.GetOutputId(Name);
        public NodeOutputAttribute? OutputAttribute { get; set; }
    }
}