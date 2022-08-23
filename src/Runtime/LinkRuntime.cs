using System;
using System.Threading.Tasks;

namespace Teriyaki.GraphFlow.Runtime
{
    public class LinkRuntime : ILinkRuntime
    {
        public string Id => Link.Id;

        public GraphLinkModel Link { get; private set; }

        public virtual Task Init(GraphRuntime graphRuntime, GraphLinkModel link, OutputRuntime? output, InputRuntime? input)
        {
            Link = link;
            //
            Input = input;
            Output = output;
            //
            return Task.CompletedTask;
        }
        //
        public InputRuntime? Input { get; set; }
        public OutputRuntime? Output { get; set; }

        public virtual ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }

    public interface ILinkRuntime : IAsyncDisposable
    {
        string Id { get; }
        Task Init(GraphRuntime graphRuntime, GraphLinkModel link, OutputRuntime? output, InputRuntime? input);

        GraphLinkModel Link { get; }
        InputRuntime? Input { get; set; }
        OutputRuntime? Output { get; set; }
    }
}