using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teriyaki.GraphFlow.Nodes;

namespace Teriyaki.GraphFlow.Tests
{
    public static class GraphModelExtensions
    {
        public static (string constStringNodeId, string inputValueNodeId) CreateSimpleGraph(this GraphModel @this)
        {
            const string constValue = "Test";
            const string constStringNodeId = "constStringNodeId";
            const string inputValueNodeId = "inputValueNodeId";
            //
            var constStringNode = @this.AddNode(new ConstStringNode()
            {
                Id = constStringNodeId,
                PropertiesData = new ConstStringNode.Properties()
                {
                    Value = constValue
                }
            });
            var inputValueNode = @this.AddNode(new InputValueNode()
            {
                Id = inputValueNodeId
            });
            var testLink = @this.AddLink(new()
            {
                OutputId = constStringNode.GetOutputId(q => q.Value),
                InputId = inputValueNode.GetInputId(q => q.Value),
            });
            //
            return new()
            { 
                constStringNodeId = constStringNodeId,
                inputValueNodeId = inputValueNodeId
            };
        }
    }
}
