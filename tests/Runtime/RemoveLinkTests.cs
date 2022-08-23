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
    public class RemoveLinkTests : GraphTestsBase
    {
        public RemoveLinkTests(ITestOutputHelper output)
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
            var testLink = _graph.AddLink(new()
            {
                OutputId = constStringNode.GetOutputId(q => q.Value),
                InputId = inputValueNode.GetInputId(q => q.Value),
            });
            //
            var runtime = CreateGraph();
            //
            runtime.StepsCount.ShouldBe(2);
            //
            await runtime.RemoveLink(testLink.Id);
            //
            runtime.StepsCount.ShouldBe(1);
        }
    }
}
