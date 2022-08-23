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
    public class ChangeNodeIdTests : GraphTestsBase
    {
        public ChangeNodeIdTests(ITestOutputHelper output)
            : base(output)
        {
        }
        
    }
}
