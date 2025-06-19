# StateGrapher
## Description
This is a tool used to graph Hierarchical State Machines.
Based on the graphs, C# state-machine classes are generated.
![](/docs/img/AppPreview.png)

Made with WPF with a great contribution of [nodify](https://github.com/miroiu/nodify).

## Generated State Machine
General structure of generated class is inspired by what [StateSmith](https://github.com/StateSmith/StateSmith) does.
The main difference is that the graph itself DOESN'T contain ANY code logic, only states and transitions.
The generated class will have a bunch of partial methods without definition that can be defined to provide actual logic.

## Terminology
### Connection
A connection between nodes in graph.

#### Two-way connections
TODO

### Transition
Change of one state to the other due to an event.

#### Initial transition
TODO

#### Exit transition
TODO

### Connection VS Transition 
Connection is visual. It's what's being created in the graph. It doesn't *necessary* reflect transition.

Let's look at some examples.

![ConnectionVSTransition](docs/img/ConnectionVSTransition.png)

Here, the "Do" connection connects "Left" state with "Right" state, but the state machine generator recognizes it as a transition from "Left" to "Sub" through "Do".

![ConnectionVSTransition1](docs/img/ConnectionVSTransition1.png)

Here, the "MoveOut" connection connects "Composite" to "Simple", but the state machine generator recognizes:
- Transition from "Left" to "Simple" through "MoveOut", and
- Transition from "Right" to "Simple" through "MoveOut"

### State
State types:
- Simple state - state without sub-states.
- State Machine (Composite state) - a state containing other states.
