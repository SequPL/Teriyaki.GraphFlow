using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Teriyaki.GraphFlow.Models.Graph;
using System.Runtime;
using FluentValidation;
using MediatR;
using Serilog;
using Teriyaki.GraphFlow.Exceptions;
using Teriyaki.GraphFlow.Models.Graph.Attributes;
using Teriyaki.GraphFlow.Models.Outputs;

namespace Teriyaki.GraphFlow.Runtime
{

    public class GraphRuntime : IAsyncDisposable
    {
        public GraphModel Graph { get; private set; }

        private ICollection<ILinkRuntime> _links { get; init; } = new HashSet<ILinkRuntime>();
        private ICollection<INodeRuntime> _nodes { get; init; } = new HashSet<INodeRuntime>();

        private Dictionary<string, LinkSlotRuntime> _slots = new();
        private ConcurrentDictionary<uint, INodeRuntime[]> Steps { get; } = new();

        private object linksLock = new object();

        //
        private readonly IServiceProvider _services;
        public long CurrentStep { get; private set; } = -1;
        public long StepsCount  => Steps.Count;
        public long LinksCount  => _links.Count;
        public long NodesCount  => _nodes.Count;
        public long SlotsCount  => _slots.Count;

        public bool AllowUnresolvedHandlers = false;

        public GraphRuntime(IServiceProvider services)
        {
            _services = services;
        }

        public async Task Load(GraphModel graph)
        {
            if (Graph != null) throw new Exception("Graph is already loaded.");
            //
            Graph = graph;

            // 1. AddNodes
            foreach (var node in Graph.Nodes)
                await AddNode(node, false, true);

            // 2. AddLinks
            foreach (var link in Graph.Links)
                await AddLink(link, false, true);

            // 3. Reorder steps
            ReorderSteps();
        }

        private void ReorderSteps()
        {
            // każdy node dostaje order który jest tożsamy z maksymalnym odstępem od wejścia 
            //
            foreach (var nodeRuntime in _nodes)
            {
                nodeRuntime.LinksToStart = !nodeRuntime.Inputs.Any() ? 0 : nodeRuntime.Inputs.Max(i => GetLinksToStart(i));
            }
            //
            Steps.Clear();
            var runtimes = _nodes.GroupBy(q => q.LinksToStart).OrderBy(q => q.Key).ToArray();
            for (uint i = 0; i < runtimes.Length; ++i)
            {
                var a = runtimes[i].ToArray();
                Steps.TryAdd(i, a);
            }
        }

        public uint GetLinksToStart(InputRuntime linkSlot, uint iterator = 0)
        {
            var links = GetLinksForLinkSlot(linkSlot);
            var graphLinkModels = links as LinkRuntime[] ?? links.ToArray();
            if (!graphLinkModels.Any())
                return iterator;
            else
            {
                uint max = 0;
                //
                foreach (LinkRuntime l in graphLinkModels)
                foreach (InputRuntime i in l.Output.Node.Inputs)
                {
                    uint u = GetLinksToStart(i, iterator + 1);
                    max = Math.Max(max, u);
                }
                //
                return max;
            }
        }


        public IEnumerable<ILinkRuntime> GetLinksForLinkSlot([NotNull] LinkSlotRuntime linkSlot)
        {
            return linkSlot switch
            {
                InputRuntime input => GetLinksForInput(input),
                OutputRuntime output => GetLinksForOutput(output),
                _ => throw new NotSupportedException(linkSlot.GetType().Name)
            };
        }

        public IEnumerable<ILinkRuntime> GetLinksForOutput(OutputRuntime output)
        {
            lock (linksLock) return _links.Where(x => x.OutputId() == output.Id).ToList();
        }

        public IEnumerable<ILinkRuntime> GetLinksForInput(InputRuntime input)
        {
            lock (linksLock) return _links.Where(x => x.InputId() == input.Id).ToList();
        }

        public virtual async Task RunOnce()
        {
            while (Steps.Count > CurrentStep)
                await RunStep();
        }

        public virtual async Task RunStep()
        {
            ++CurrentStep;
            //
            if (Steps.Count > CurrentStep)
            {
                var steps = Steps[(uint) CurrentStep];
                Log.Information("Begin step {CurrentStep} with items {count}", CurrentStep, steps.Length);
                //
                try
                {
                    await Task.WhenAll(steps
                        .Select(RunForNode)
                        .ToArray());
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Steps loop.");
                    //
                    --CurrentStep;
                    throw;
                }
                //
                Log.Information("End step {CurrentStep} with items {count}", CurrentStep, steps.Length);
            }
            else
            {
                Log.Information("Steps's stack ended on step : {count}", CurrentStep);
                CurrentStep = (uint)Steps.Count;
            }                
        }

        protected virtual async Task<InputRuntime[]> RunForNode(INodeRuntime runtime)
        {
            var triggeredInputs = new List<InputRuntime>();
            //
            Log.Debug("Executing node : {nodeId}", runtime.Node.Id);

            // 1. uruchomienie
            try { await runtime.Execute(); }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Executing {nodeId} of type : {nodeType}", runtime.Node.Id, runtime.Node.GetType());
                //
                throw;
            }

            // 2. propagacja 
            foreach (var output in runtime.Outputs) triggeredInputs.AddRange(await PropagateOutputValue(output));
            //
            Log.Debug("Executed node : {nodeId}", runtime.Node.Id);
            //
            return triggeredInputs.ToArray();
        }

        protected virtual Task<InputRuntime[]> PropagateOutputValue(OutputRuntime output)
        {
            var triggeredInputs = new List<InputRuntime>();
            //
            var value = output.GetValue();
            //
            Log.Debug("Propagate output value for : {output} with links : {count}", output, output.Links.Count);
            //
            foreach (var link in output.Links)
            {
                if (link.Input != null)
                {
                    link.Input.SetValue(value);
                    triggeredInputs.Add(link.Input);
                }
            }
            //
            return Task.FromResult(triggeredInputs.ToArray());
        }

        public NodeRuntime<TNode>? GetNodeRuntime<TNode>(TNode graphNode)
            where TNode : class, IGraphNodeModel
        {
            return GetNodeRuntime<TNode>(graphNode.Id);
        }

        public NodeRuntime<TNode>? GetNodeRuntime<TNode>(string graphNodeId)
            where TNode : class, IGraphNodeModel
        {
            var runtime = _nodes.FirstOrDefault(q => q.Node.Id == graphNodeId);
            //
            if (runtime is NodeRuntime<TNode> typedRuntime)
                return typedRuntime;
            else
                throw new Exception($"Runtime is typed as {runtime.GetType()} but requested for {typeof(TNode)}");
        }
        public INodeRuntime? GetNodeRuntime(string nodeId)
        {
            return _nodes.FirstOrDefault(q => q.Node.Id == nodeId);
        }
        public INodeHandler CreateNodeHandler([DisallowNull] IGraphNodeModel node)
        {
            var handlerType = node.GetHandlerType();
            var handler     = _services.GetService(handlerType) as INodeHandler;
            //
            if (handler != null) return handler;
            //
            if (AllowUnresolvedHandlers)
                return null;
            else
                throw new Exception($"Cannot create handler {handlerType.FullName}, because not found handler service. Node : {node.GetType().Name}");
        }

        public async Task TriggerOutputSlot(OutputRuntime output)
        {
            var triggeredInputs = await PropagateOutputValue(output);
            //
            foreach (var input in triggeredInputs)
            {
                await RunForNode(input.Node);
                //
                foreach (var o in input.Node.Outputs)
                {
                    await PropagateOutputValue(o);
                    await TriggerOutputSlot(o);
                }
            }
        }

        public Task<INodeRuntime> AddNode(IGraphNodeModel node)
        {
            return AddNode(node, true, false);
        }
        public Task<ILinkRuntime> AddLink(GraphLinkModel link)
        {
            return AddLink(link, true, false);
        }

        private async Task<INodeRuntime> AddNode(IGraphNodeModel node, bool invalidGraph, bool lockAddToGraphModel)
        {            
            var runtime = CreateNodeRuntime(node);// _services.GetService(runtimeType) as INodeRuntime ?? throw new Exception($"Cannot create runtime for {runtimeType.Name}, because not found runtime service.");
            //
            await runtime.Init(this, node);
            //
            foreach (var runtimeInput in runtime.Inputs) { _slots.Add(runtimeInput.Id, runtimeInput); }
            foreach (var runtimeOutput in runtime.Outputs) { _slots.Add(runtimeOutput.Id, runtimeOutput); }
            //
            _nodes.Add(runtime);
            //
            if (invalidGraph)
            {
                foreach (var link in _links)
                {
                    if (link.Input == null)
                    {
                        link.Input = runtime.Inputs.FirstOrDefault(q => q.Id == link.InputId());
                        //
                        if(link.Input != null)
                            link.Input.Links.Add(link);
                    }

                    if (link.Output == null)
                    {
                        link.Output = runtime.Outputs.FirstOrDefault(q => q.Id == link.OutputId());
                        //
                        if (link.Output != null)
                            link.Output.Links.Add(link);
                    }
                }
                //
                ReorderSteps();
            }
            //
            if(!lockAddToGraphModel)
                Graph.Nodes.Add(node);
            //
            return runtime;
        }

        private async Task<ILinkRuntime> AddLink(GraphLinkModel link, bool reorderSteps, bool lockAddToGraphModel)
        {
            if (!_slots.ContainsKey(link.InputId) && !Graph.AllowMissingSlots) throw new MissingLinkSlotException(link.InputId, link.OutputId);
            if (!_slots.ContainsKey(link.OutputId) && !Graph.AllowMissingSlots) throw new MissingLinkSlotException(link.InputId, link.OutputId);
            //
            _slots.TryGetValue(link.InputId, out var input);
            _slots.TryGetValue(link.OutputId, out var output);
            //
            var runtime = CreateLinkRuntime(link);
            //
            await runtime.Init(this, link, output as OutputRuntime, input as InputRuntime);
            //
            output?.Links.Add(runtime);
            input?.Links.Add(runtime);
            //
            _links.Add(runtime);
            //
            if(reorderSteps)
                ReorderSteps();
            //
            if (!lockAddToGraphModel)
                Graph.Links.Add(link);
            //
            return runtime;
        }

        private INodeRuntime CreateNodeRuntime(IGraphNodeModel node)
        {
            return GetService<INodesFactory>().CreateNodeRuntime(node);
            //return _services.GetService(node) as INodeRuntime ?? throw new Exception($"Cannot create runtime for {node.GetType().Name}.");
        }
        private ILinkRuntime CreateLinkRuntime(GraphLinkModel link)
        {
            return GetService<ILinksFactory>().CreateLinkRuntime(link);

            //var runtimeType = typeof(ILinkRuntime);
            //return _services.GetService(runtimeType) as ILinkRuntime ?? throw new Exception($"Cannot create runtime for {runtimeType.Name}, because not found runtime service.");
        }

        public async Task RemoveLink(string linkId)
        {
            var runtime = GetLinkRuntime(linkId) ?? throw new KeyNotFoundException(linkId);
            //
            await runtime.DisposeAsync();
            //
            if (runtime.Input != null) runtime.Input.Links.Remove(runtime);
            if (runtime.Output != null) runtime.Output.Links.Remove(runtime);
            //
            _links.Remove(runtime);
            Graph.Links.Remove(runtime.Link);
            //
            ReorderSteps();
        }
        public async Task RemoveNode(string nodeId)
        {
            var runtime = GetNodeRuntime(nodeId) ?? throw new KeyNotFoundException(nodeId);
            //
            await runtime.DisposeAsync();
            //
            foreach (var runtimeInput in runtime.Inputs)
            {
                _slots.Remove(runtimeInput.Id);
                foreach (var link in runtimeInput.Links.ToArray())
                    await RemoveLink(link.Id);
            }

            foreach (var runtimeOutput in runtime.Outputs)
            {
                _slots.Remove(runtimeOutput.Id);
                foreach (var link in runtimeOutput.Links.ToArray())
                    await RemoveLink(link.Id);
            }
            //
            _nodes.Remove(runtime);
            Graph.Nodes.Remove(runtime.Node);
            //
            ReorderSteps();
        }
        public ILinkRuntime? GetLinkRuntime(string linkId)
        {
            return _links.FirstOrDefault(q => q.Id == linkId);
        }

        public Task ResetSteps()
        {
            CurrentStep = -1;
            //
            return Task.CompletedTask;
        }
        public TService GetService<TService>() where TService : notnull => _services.GetRequiredService<TService>();

        public async ValueTask DisposeAsync()
        {
            foreach (var link in _links) {  await link.DisposeAsync(); }
            foreach (var node in _nodes) { await node.DisposeAsync(); }
            //
            _slots.Clear();
            Steps.Clear();
        }
    }
}