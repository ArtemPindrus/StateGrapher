using StateGrapher.Extensions;
using StateGrapher.Models;
using System.Text.RegularExpressions;

namespace StateGrapher.Utilities {
    public static class StateMachineUtility {
        public static StateMachine? GetLeastCommonAncestor(Transition transition) {
            var fromParentTree = transition.From.GetParentTree().ToArray();
            var toParentTree = transition.To.GetParentTree().ToArray();

            if (fromParentTree.Length > toParentTree.Length) return Traverse(fromParentTree, toParentTree);
            else return Traverse(toParentTree, fromParentTree);

                StateMachine? Traverse(IEnumerable<StateMachine> bigger, IEnumerable<StateMachine> smaller) {
                    foreach (var i in bigger) {
                        var found = smaller.FirstOrDefault(x => x == i);

                        if (found != null) return found;
                    }

                    return null;
                }
        }

        public static List<Transition> ToTransitions(IEnumerable<Connection> allConnections) {
            List<Transition> result = new();

            foreach (var connection in allConnections) {
                ConnectionToTransition(connection, allConnections, result);
            }

            // consume ancestor events
            var same = result.GroupBy(x => new { x.From, x.Name })
                .Where(g => g.Count() > 1);

            foreach (var key in same) {
                var mostNested = GetMostNested(key.Select(x => x.To));

                foreach (var t in key) {
                    if (t.To != mostNested) result.Remove(t);
                }
            }

            return result;
        }

        public static bool ConnectionToTransition(Connection connection, IEnumerable<Connection> allConnections, List<Transition> transitions) {
            // skip connection to InitialState
            if (connection.From.Container is InitialState) {
                return false;
            }

            // skip "via exit" connections
            if (connection.ForwardEvent != null && connection.ForwardEvent.StartsWith("via exit")) return false;

            if (TryGetExitTransition(connection) is Transition exitTransition) {
                transitions.Add(exitTransition);
                return true;
            }

            if (connection.From.Container is StateMachine 
                && connection.To.Container is StateMachine) {
                TryGetFromInterstateConnection(connection, transitions);
            }

            // fallback
            transitions = new();
            return false;

            Transition? TryGetExitTransition(Connection connection) {
                if (connection.To.Container is ExitNode exitNode) {
                    if (connection.ForwardEvent == null
                        || connection.Container == null) return null;

                    var from = (StateMachine)connection.From.Container;

                    string pattern = @$"^via exit {exitNode.Name}$";

                    Connection? exitConnection = allConnections
                        .FirstOrDefault(x => x.ForwardEvent != null && Regex.IsMatch(x.ForwardEvent, pattern));

                    if (exitConnection == null) return null;

                    var to = (StateMachine)exitConnection.To.Container;

                    return new Transition(connection.ForwardEvent, from, to, connection.Container);
                }

                return null;
            }
            void TryGetFromInterstateConnection(Connection connection, List<Transition> transitions) {
                if (connection.Container == null) return;

                StateMachine from = (StateMachine)connection.From.Container;
                StateMachine to = (StateMachine)connection.To.Container;

                // forward event
                if (!string.IsNullOrEmpty(connection.ForwardEvent)) {
                    Try_Internal(from, to, connection.ForwardEvent);
                }

                // back event
                if (!string.IsNullOrEmpty(connection.BackEvent)) {
                    Try_Internal(to, from, connection.BackEvent);
                }

                void Try_Internal(StateMachine from, StateMachine to, string eventName) {
                    if (!string.IsNullOrEmpty(connection.ForwardEvent)) {
                        if (from.Nodes.Count == 0) { // from simple
                            if (GetEntry(to) is StateMachine entry) {
                                transitions.Add(new(eventName, from, entry, connection.Container));
                            }
                        } else { // from state machine
                            if (GetEntry(to) is StateMachine entry) {
                                AddNested(from, entry);
                            }
                        }
                    }

                    void AddNested(StateMachine from, StateMachine to) {
                        foreach (var n in from.Nodes.OfType<StateMachine>()) {
                            if (n.Nodes.Count == 0) transitions.Add(new(eventName, n, to, connection.Container));
                            else {
                                AddNested(n, to);
                            }
                        }
                    }
                } 
            }
        }
    
        public static StateMachine GetMostNested(IEnumerable<StateMachine> states) {
            return states.OrderByDescending(x => x.GetParentTree().Count()).First();
        }

        public static StateMachine? GetEntry(StateMachine stateMachine) {
            if (stateMachine.Nodes.Count == 0) {
                return stateMachine;
            } else if (stateMachine.InitialState is InitialState ins) {
                return ins.Connection.To.Container as StateMachine;
            }

            return null;
        }    
    }
}
