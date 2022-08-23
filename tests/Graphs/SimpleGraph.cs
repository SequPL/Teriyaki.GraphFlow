using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Shouldly;
using Teriyaki.GraphFlow.NodeHandlers;
using Teriyaki.GraphFlow.Nodes;
using Teriyaki.GraphFlow.Runtime;
using Xunit;
using Xunit.Abstractions;

namespace Teriyaki.GraphFlow.Tests.Runtime
{
    public class SimpleGraph : GraphTestsBase
    {
        public SimpleGraph(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task SimpleRun_bySteps()
        {
            const string constValue = "Test";
            const string constStringNodeId = "constStringNodeId";
            const string inputValueNodeId = "inputValueNodeId";
            //
            var constStringNode = _graph.AddNode(new ConstStringNode()
            {
                Id = constStringNodeId,
                PropertiesData = new ConstStringNode.Properties()
                {
                    Value = constValue
                }
            });
            var inputValueNode = _graph.AddNode(new InputValueNode()
            {
                Id = inputValueNodeId
            });
            //
            _graph.AddLink(new()
            {
                OutputId = constStringNode.GetOutputId(q => q.Value),
                InputId = inputValueNode.GetInputId(q => q.Value),
            });
            //
            var runtime = CreateGraph();
            //
            await runtime.RunOnce();
            //
            var nodeRuntime = runtime.GetNodeRuntime(inputValueNode);
            var runtimeValue = await nodeRuntime?.GetInputValue<InputValueNode.Inputs>(q => q.Value);
            //
            runtimeValue.ShouldBe(constValue);
        }

        [Fact]
        public async Task SimpleRun_byTrigger()
        {
            const string buttonNodeId = "buttonNodeId";
            const string inputValueNode1Id = "inputValueNode1Id";
            const string inputValueNode2Id = "inputValueNode2Id";

            // 1. Init
            var buttonNode = _graph.AddNode(new ButtonNode()
            {
                Id = buttonNodeId
            });
            var inputValueNode1 = _graph.AddNode(new InputValueNode()
            {
                Id = inputValueNode1Id
            });
            var inputValueNode2 = _graph.AddNode(new InputValueNode()
            {
                Id = inputValueNode2Id
            });
            _graph.AddLink(new()
            {
                OutputId = buttonNode.GetOutputId(q => q.IsClicked),
                InputId = inputValueNode1.GetInputId(q => q.Value),
            });
            _graph.AddLink(new()
            {
                OutputId = inputValueNode1.GetOutputId(q => q.Value),
                InputId = inputValueNode2.GetInputId(q => q.Value),
            });
            //
            var runtime = CreateGraph();
            //
            var inputValueNodeRuntime1 = runtime.GetNodeRuntime<InputValueNode>(inputValueNode1.Id);
            var inputValueNodeRuntime2 = runtime.GetNodeRuntime<InputValueNode>(inputValueNode2.Id);
            var buttonNodeRuntime = runtime.GetNodeRuntime<ButtonNode>(buttonNode.Id);

            // 2. Act nothing
            await runtime.RunOnce();

            // Assert
            var inputValueNodeRuntimeValue = await inputValueNodeRuntime1?.GetInputValue<InputValueNode.Inputs>(q => q.Value);
            inputValueNodeRuntimeValue.ShouldBe("False");

            // 3. Act trigger
            await (buttonNodeRuntime.Handler as ButtonNodeHandler).OnMouseDown(buttonNodeRuntime);

            // Assert
            inputValueNodeRuntimeValue = await inputValueNodeRuntime1?.GetInputValue<InputValueNode.Inputs>(q => q.Value);
            inputValueNodeRuntimeValue.ShouldBe("True");
            //
            inputValueNodeRuntimeValue = await inputValueNodeRuntime2?.GetInputValue<InputValueNode.Inputs>(q => q.Value);
            inputValueNodeRuntimeValue.ShouldBe("True");
        }
    }
}