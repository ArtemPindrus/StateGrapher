using StateGrapher.Models;

namespace StateGrapher.Extensions
{
    public static class StateMachineExtensions {
        public static IEnumerable<StateMachine> GetHierarchyNodes(this StateMachine sm, bool includeSelf = false) {
            if (includeSelf) yield return sm;

            foreach (var node in sm.Nodes.OfType<StateMachine>()) {
                yield return node;

                foreach (var child in GetHierarchyNodes(node)) yield return child;
            }
        }

        public static IEnumerable<Connection> GetHierarchyConnections(this StateMachine sm) {
            foreach (var connection in sm.Connections) yield return connection;

            foreach (var childNode in sm.GetHierarchyNodes()) {
                foreach (var childConnection in GetHierarchyConnections(childNode)) yield return childConnection;
            }
        }
    }
}
