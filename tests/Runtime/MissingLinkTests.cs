using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Shouldly;
using Teriyaki.GraphFlow.Exceptions;
using Teriyaki.GraphFlow.Nodes;
using Teriyaki.GraphFlow.Runtime;
using Xunit;
using Xunit.Abstractions;

namespace Teriyaki.GraphFlow.Tests.Serialization
{
    public class MissingLinkTests : GraphTestsBase
    {
        public MissingLinkTests(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task AllowMissingSlots()
        {
            // Powinno zostać zdeserializowane z warningiem
            //
            const string constStringNodeId = "constStringNodeId";
            const string inputValueNodeId = "inputValueNodeId";
            //
            var constStringNode = _graph.AddNode(new ConstStringNode()
            {
                Id = constStringNodeId
            });
            var inputValueNode = _graph.AddNode(new InputValueNode()
            {
                Id = inputValueNodeId
            });
            //
            _graph.AddLink(new()
            {
                OutputId = constStringNode.GetOutputId(q => q.Value),
                InputId = inputValueNode.GetInputId("Not existing input"),
            });
            //
            var container = _servicesBuilder.Build();
            var provider = new AutofacServiceProvider(container);
            //
            var gruntime = new GraphRuntime(provider);
            //
            ShouldThrowExtensions.ShouldThrow<MissingLinkSlotException>(() => gruntime.Load(_graph).GetAwaiter().GetResult());
            //
            var gruntime2 = new GraphRuntime(provider);
            _graph.AllowMissingSlots = true;
            await gruntime2.Load(_graph);
        }
    }
}
