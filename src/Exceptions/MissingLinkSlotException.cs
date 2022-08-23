using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teriyaki.GraphFlow.Exceptions
{
    public class MissingLinkSlotException : Exception
    {
        public MissingLinkSlotException(string inputId, string outputId)
            : base($"Missing slot from input {inputId}, to {outputId}.")
        {

        }
    }
}
