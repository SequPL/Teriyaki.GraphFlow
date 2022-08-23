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
    public class RemoveNodeTests : GraphTestsBase
    {
        public RemoveNodeTests(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task ClearAll()
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
            runtime.NodesCount.ShouldBe(2);
            runtime.LinksCount.ShouldBe(1);
            //
            await runtime.RemoveNode(constStringNodeId);
            //
            runtime.NodesCount.ShouldBe(1);
            runtime.LinksCount.ShouldBe(0);
            runtime.SlotsCount.ShouldBe(3); // InputValueNode - > Inputs.Enable, Inputs.Value, Outputs.Value
            //
            runtime.Graph.Nodes.Count.ShouldBe(1);
            runtime.Graph.Links.Count.ShouldBe(0);
        }
    }
}
