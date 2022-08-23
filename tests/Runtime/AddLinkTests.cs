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
    public class AddLinkTests : GraphTestsBase
    {
        public AddLinkTests(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task StaticSlot()
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
            var runtime = CreateGraph();
            //
            await runtime.RunOnce();
            //
            runtime.StepsCount.ShouldBe(1);
            //
            await runtime.AddLink(new()
            {
                OutputId = constStringNode.GetOutputId(q => q.Value),
                InputId = inputValueNode.GetInputId(q => q.Value),
            });
            //
            runtime.StepsCount.ShouldBe(2);
            //
            await runtime.ResetSteps();
            await runtime.RunOnce();
            //
            var nodeRuntime = runtime.GetNodeRuntime(inputValueNode);
            var runtimeValue = await nodeRuntime?.GetInputValue<InputValueNode.Inputs>(q => q.Value);
            //
            runtimeValue.ShouldBe(constValue);
        }
    }
}
