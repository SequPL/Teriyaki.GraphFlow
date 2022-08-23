using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using Teriyaki.GraphFlow.Models.Inputs;
using Teriyaki.GraphFlow.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Teriyaki.GraphFlow.Tests.Serialization
{
    public class DynamicInputSerializationTests : GraphTestsBase
    {
        public DynamicInputSerializationTests(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task DynamicInputsShouldExists()
        {
            const string ifNodeId = "ifNodeId";
            const string inputValueNodeId = "inputValueNodeId";
            //
            var constNumberValueNode = _graph.AddNode(new ConstNumberNode()
            {
                PropertiesData = new()
                {
                    Value = 5L
                }
            });
            var constNumberValueNode2 = _graph.AddNode(new ConstNumberNode()
            {
                PropertiesData = new()
                {
                    Value = 2L
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
                    DynamicInputs = new()
                    {
                        {"A", 5L},
                        {"b", 2L}
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
            var serializationSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var serializedGraph = JsonConvert.SerializeObject(_graph, Formatting.Indented, serializationSettings);
            var deserializedGraph = JsonConvert.DeserializeObject<GraphModel>(serializedGraph, serializationSettings);
            //
            deserializedGraph.ShouldBeEquivalentTo(_graph);
        }
    }
}