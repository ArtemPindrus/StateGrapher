using StateGrapher.Models;
using StateGrapher.ViewModels;

namespace StateGrapher.Extensions
{
    public static class StateMachineExtensions {
        /// <summary>
        /// Get all <see cref="StateMachine"/> nodes.
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="includeSelf">Whether to include self (<paramref name="sm"/>).</param>
        /// <returns></returns>
        public static IEnumerable<StateMachine> GetHierarchyNodes(this StateMachine sm, bool includeSelf = false) {
            if (includeSelf) yield return sm;

            foreach (var node in sm.Nodes.OfType<StateMachine>()) {
                yield return node;

                foreach (var child in GetHierarchyNodes(node)) yield return child;
            }
        }

        public static Connection? TryAddConnection(this StateMachine root, string fromState, string toState) {
            StateMachine? from = root.Nodes.FirstOrDefault(x => x.Name == fromState) as StateMachine;
            StateMachine? to = root.Nodes.FirstOrDefault(x => x.Name == toState) as StateMachine;

            if (from != null && to != null) {
                return root.TryAddConnection(from.Connectors.TopConnectors[0], to.Connectors.TopConnectors[0]);
            }

            return null;
        }

        public static IEnumerable<StateMachine> GetParentTree(this StateMachine sm) {
            var parent = sm.Container;

            while (parent != null) {
                yield return parent;
                parent = parent.Container;
            }
        }

        /// <summary>
        /// Get all <see cref="StateMachine"/> nodes.
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="includeSelf">Whether to include self (<paramref name="sm"/>).</param>
        /// <returns></returns>
        public static IEnumerable<StateMachineViewModel> GetHierarchyNodes(this StateMachineViewModel sm, bool includeSelf = false) {
            if (includeSelf) yield return sm;

            foreach (var node in sm.Nodes.OfType<StateMachineViewModel>()) {
                yield return node;

                foreach (var child in GetHierarchyNodes(node)) yield return child;
            }
        }

        public static StateMachine TryAddNodes(this StateMachine sm, params Node[] nodes) {
            foreach (var node in nodes) {
                sm.TryAddNode(node);
            }

            return sm;
        }

        public static IEnumerable<Connection> GetHierarchyConnections(this StateMachine sm) {
            foreach (var connection in sm.Connections) yield return connection;

            foreach (var childNode in sm.GetHierarchyNodes()) {
                foreach (var childConnection in GetHierarchyConnections(childNode)) yield return childConnection;
            }
        }
    }
}
