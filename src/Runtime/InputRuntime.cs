namespace Teriyaki.GraphFlow.Runtime
{
    public record InputRuntime : LinkSlotRuntime
    {
        public override string Id => Node.Node.GetInputId(Name);
        public NodeInputAttribute? InputAttribute { get; set; }
        public bool IsDynamic { get; set; }
    }
}