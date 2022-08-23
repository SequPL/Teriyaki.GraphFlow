using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Teriyaki.GraphFlow.Runtime;

namespace Teriyaki.GraphFlow
{
    public interface INodeRuntime : IAsyncDisposable
    {
        Task Init(GraphRuntime graphRuntime, IGraphNodeModel node);
        Task Execute();
        //
        IGraphNodeModel Node { get; }
        INodeHandler Handler { get; }
        //
        IReadOnlySet<InputRuntime> Inputs { get; }
        IReadOnlySet<OutputRuntime> Outputs { get; }
        IReadOnlySet<PropertyRuntime> Properties { get; }
        //
        uint LinksToStart { get; set; }
        bool Enable { get; }
    }
}