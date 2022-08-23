using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using Teriyaki.GraphFlow.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Teriyaki.GraphFlow.Tests.Serialization
{
    public class SimpleGraphSerializationTests : GraphTestsBase
    {
        public SimpleGraphSerializationTests(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task SimpleSerialization()
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
            var serializedGraph = JsonConvert.SerializeObject(_graph, Formatting.Indented);
            _graph = JsonConvert.DeserializeObject<GraphModel>(serializedGraph);
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
    }
}
