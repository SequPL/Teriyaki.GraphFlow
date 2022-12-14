# Teriyaki.GraphFlow
Main goals :
- simple graph creation 
- node's handlers must supports microservices ( for dynamic load-balance, and DDD polices )
- many graph enters ( events-based )
- graphs must be serializable
- "fluent" node definions

# Example
Graph creation from code :
``` C#
var graph = new GraphModel();
//
const string constValue = "Test";

// add node #1 <- ConstStringNode
var constStringNode = graph.AddNode(new ConstStringNode()
{
    PropertiesData = new ConstStringNode.Properties()
    {
        Value = constValue
    }
});

// add node #2 <- InputValueNode
var inputValueNode = graph.AddNode(new InputValueNode());

// add link between node #1 and #2
graph.AddLink(new()
{
    OutputId = constStringNode.GetOutputId(q => q.Value),
    InputId = inputValueNode.GetInputId(q => q.Value),
});
```

Run Graph : 
``` C#
// 1. Create runtime :
var runtime = new GraphRuntime(servicesProvider); // implementation of IServiceProvider <- you can use e.g. AutofacServiceProvider

// 2. load graph to runtime
await runtime.Load(_graph);

// 3. run graph :
await runtime.RunOnce();

```

# Create new node definition
Nodes may have : 
- state
- outputs
- inputs 
- properties 
- layout ( for ui purposes )

see: Teriyaki.GraphFlow.GraphNodeModel.cs

example : 
``` c#
public class InputValueNode : GraphNodeModel<InputValueNode, InputValueNode.Inputs, InputValueNode.Outputs>
    {
        public class Inputs : GraphNodeBasicInputs
        {
            public string Value { get; set; }
        }
        public class Outputs : GraphNodeBasicOutputs
        {
            public string Value { get; set; }
        }
    }
````

This node has inputs and outputs. 

# Create node handler
Node is only definion of something. To handle it and do something you need NodeHandler.

Example :
``` C#
public class InputValueNodeHandler : InputValueNode.HandlerBase
    {
        public override async Task OnExecute(IRuntimeContext c)
        {
            await SetOutputFromInput(c, q => q.Value, q => q.Value);
            await SetPropertyValue(c, q => q.IsExecuted, true);
            //
            await base.OnExecute(c);
        }
    }
```
It can do anything, but main functions are :
- SetOutputFromInput
- SetOutputFromProperty
- SetOutputValue
- SetPropertyValue
- TriggerOutputSlot

# Coming soon 
- Teriyaki.GraphFlow.LiteGraph - ui for editing graphs ( with https://github.com/jagenjo/litegraph.js ), something like : ![MyNodes](https://github.com/jagenjo/litegraph.js/raw/master/imgs/mynodes.png "MyNodes")

