using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Shouldly;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Teriyaki.GraphFlow.Tests.Nodes
{
    public class IfNodeTests : GraphTestsBase
    {
        public IfNodeTests(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task Result()
        {
            const string ifNodeId = "ifNodeId";
            const string inputValueNodeId = "inputValueNodeId";
            //
            var constNumberValueNode = _graph.AddNode(new ConstNumberNode()
            {
                PropertiesData = new()
                {
                    Value = 5
                }
            });
            var constNumberValueNode2 = _graph.AddNode(new ConstNumberNode()
            {
                PropertiesData = new()
                {
                    Value = 2
                }
            });
            var ifNode = _graph.AddNode(new IfNode()
            {
                Id = ifNodeId,
                PropertiesData = new()
                {
                    Condition = "A > b"
                },
                InputsData = new()
                {
                    DynamicInputs = new ()
                    {
                        { "A", 5L },
                        { "b", 2L }
                    }
                }
            });
            //
            var inputValueNode = _graph.AddNode(new InputValueNode()
            {
                Id = inputValueNodeId
            });
            //
            _graph.AddLink(new()
            {
                OutputId = constNumberValueNode.GetOutputId(q => q.Value),
                InputId = ifNode.GetInputId("A"),
            });
            _graph.AddLink(new()
            {
                OutputId = constNumberValueNode2.GetOutputId(q => q.Value),
                InputId = ifNode.GetInputId("b"),
            });
            _graph.AddLink(new()
            {
                OutputId = ifNode.GetOutputId(q => q.IsTrue),
                InputId = inputValueNode.GetInputId(q => q.Value),
            });
            //
            var runtime = CreateGraph();
            //
            var ifNodeRuntime = runtime.GetNodeRuntime(ifNode);
            var nodeRuntime = runtime.GetNodeRuntime(inputValueNode);
            //
            await runtime.RunOnce();
            //
            ifNodeRuntime.GetInputValue("A").Result.ShouldBe(5);
            ifNodeRuntime.GetInputValue("b").Result.ShouldBe(2);
            //
            var runtimeValue = await nodeRuntime?.GetInputValue<InputValueNode.Inputs>(q => q.Value);
            //
            runtimeValue.ShouldBe("True");
        }

        [Fact]
        public async Task ConditionalSlots()
        {
            const string ifNodeId = "ifNodeId";
            const string inputValueNodeId = "inputValueNodeId";
            const string inputValueNode2Id = "inputValueNode2Id";
            //
            var ifNode = _graph.AddNode(new IfNode()
            {
                Id = ifNodeId,
                PropertiesData = new()
                {
                    Condition = "true"
                },
            });
            //
            var inputValueNode = _graph.AddNode(new InputValueNode()
            {
                Id = inputValueNodeId
            });
            var inputValueNode2 = _graph.AddNode(new InputValueNode()
            {
                Id = inputValueNode2Id
            });
            //
            _graph.AddLink(new()
            {
                OutputId = ifNode.GetOutputId(q => q.IsTrue),
                InputId = inputValueNode.GetInputId(q => q.Enable),
            });
            _graph.AddLink(new()
            {
                OutputId = ifNode.GetOutputId(q => q.IsFalse),
                InputId = inputValueNode2.GetInputId(q => q.Enable),
            });
            //
            var runtime = CreateGraph();
            //
            await runtime.RunOnce();
            //
            var inputValueNodeRuntime = runtime.GetNodeRuntime(inputValueNode);
            var inputValueNode2Runtime = runtime.GetNodeRuntime(inputValueNode2);
            //
            (await inputValueNodeRuntime.GetPropertyValue<InputValueNode.Properties, bool>(q => q.IsExecuted)).ShouldBeTrue();
            (await inputValueNode2Runtime.GetPropertyValue<InputValueNode.Properties, bool>(q => q.IsExecuted)).ShouldBeFalse();
        }
    }
}