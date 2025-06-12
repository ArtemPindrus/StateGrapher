# StateGrapher
## Terminology
Connection:
A connection between nodes.

## State
State types:
- Simple state - state without sub-states.
- State Machine (Composite state) - a state containing other states.

### Transition
Change of one state to the other due to an event.
All transitions derive from connections, but not every connection is a transition.

While connection describes visual connection of nodes in graph, transition describes actual state change in code.

Transition types:
- Direct transition - transition between two simple states. (SimpleState > SimpleState)
- Initial transition - transition from a state to initial state of a State Machine. (State > StateMachine)
- Entry transition - transition from a state to an entry of StateMachine. (State > StateMachine > State)
- Exit transition - transition from StateMachine to a State through exit node. (State > StateMachine > State)
- General exit transition - transition from any sub-state of a StateMachine to another excluded state. (Any state > StateMachine > State)
