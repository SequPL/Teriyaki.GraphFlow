using System;

namespace Teriyaki.GraphFlow.Models.Inputs
{
    public class DynamicInputsSet : DynamicDataSet<DynamicInput>
    {
        public void Add(string name, object defaultValue)
        {
            if (defaultValue is DynamicInput di)
            {
                if (di.Type == null) throw new ArgumentNullException(nameof(DynamicInput.Type));
                //
                Add(di);
            }
            else
            {
                Add(new DynamicInput()
                {
                    Name = name,
                    Value = defaultValue,
                    Type = defaultValue?.GetType() ?? typeof(object),
                    //
                    InputAttribute = new NodeInputAttribute()
                    {
                        DefaultValue = defaultValue
                    }
                });
            }

        }
    }
}