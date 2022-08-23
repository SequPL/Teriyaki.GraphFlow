using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teriyaki.GraphFlow.Runtime;

namespace Teriyaki.GraphFlow
{
    public static class ILinkRuntimeExtensions
    {
        public static string InputId(this ILinkRuntime @this)
        {
            return @this.Input?.Id ?? @this.Link.InputId;
        }
        public static string OutputId(this ILinkRuntime @this)
        {
            return @this.Output?.Id ?? @this.Link.OutputId;
        }
    }
}
