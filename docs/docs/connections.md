---
uid: connections
---

# Connections
Nodes within a graph can be connected.

![Connections in action](/images/connectionsNodes.gif)

It's important to understand that connections between states don't necessary reflect transitions between states. (on that later)

## Forwards and backwards events


## Conditions
See [StateMachine booleans](/docs/graphoptions.html#statemachine-booleans) before reading this.

Conditions describe state of StateMachine in which events should trigger transitions.

![Connection condition in action](/images/connectionConditionsInAction.gif)

Conditions are supported for both the forwards and backwards events and have only two states: true or false.


Setting a checked condition for an event is equivalent to stating:

if (condition) EventHandler();

Setting an unchecked condition for an event is equivalent to stating:

if (!condition) EventHandler();  // note the !