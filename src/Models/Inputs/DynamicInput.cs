namespace Teriyaki.GraphFlow.Models.Inputs
{
    public class DynamicInput : DynamicSlotBase
    {
        public DynamicInput()
        {
        }

        public DynamicInput(object defaultValue)
        {
            Value = defaultValue;
            Type = defaultValue?.GetType();
            //
            InputAttribute = new NodeInputAttribute()
            {
                DefaultValue = defaultValue
            };
        }

        public NodeInputAttribute InputAttribute { get; set; }
    }
}