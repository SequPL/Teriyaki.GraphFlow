using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Teriyaki.GraphFlow.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Teriyaki.GraphFlow.Tests.Runtime
{
    public class AddMissingLinkTests : GraphTestsBase
    {
        public AddMissingLinkTests(ITestOutputHelper output)
            : base(output)
        {
        }

        // powinno zmodyfikować input / output w link 
        [Fact]
        public async Task StaticSlot()
        {
            const string constValue = "Test";
            const string constStringNodeId = "constStringNodeId";
            const string inputValueNodeId = "inputValueNodeId";
            //
            _graph.AllowMissingSlots = true;
            //
            var constStringNode = _graph.AddNode(new ConstStringNode()
            {
                Id = constStringNodeId,
                PropertiesData = new ConstStringNode.Properties()
                {
                    Value = constValue
                }
            });
            var inputValueNode = new InputValueNode()
            {
                Id = inputValueNodeId
            };
            //
            _graph.AddLink(new()
            {
                OutputId = constStringNode.GetOutputId(q => q.Value),
                InputId = inputValueNode.GetInputId(q => q.Value),
            });
            //
            var runtime = CreateGraph();
            //
            runtime.StepsCount.ShouldBe(1);
            //
            await runtime.AddNode(inputValueNode);
            //
            var nodeRuntime = runtime.GetNodeRuntime(inputValueNode);
            var linkedInput = await nodeRuntime.GetInput(nameof(InputValueNode.InputsData.Value));
            //
            linkedInput.Links.Count.ShouldBe(1);
            runtime.StepsCount.ShouldBe(2);
            //
            await runtime.ResetSteps();
            await runtime.RunOnce();
            //
            var runtimeValue = await nodeRuntime?.GetInputValue<InputValueNode.Inputs>(q => q.Value);
            //
            runtimeValue.ShouldBe(constValue);
        }
    }
}

