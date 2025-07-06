using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace StateGrapher.Models
{
    public partial class StateMachine : Node {
        private readonly ObservableCollection<Node> nodes = [];
        private readonly ObservableCollection<Connection> connections = [];

        [ObservableProperty]
        private bool isExpanded;

        public InitialState? InitialState { get; private set; }

        public ObservableCollection<Node> Nodes {
            get => nodes;
            set {
                RemoveAllNodes();
                TryAddNodes(value);
            }
        }

        public ObservableCollection<Connection> Connections {
            get => connections;
            set {
                RemoveAllConnections();
                TryAddConnections(value);
            }
        }

        public ConnectorsCollection Connectors { get; }

        public StateMachine() {
            Connectors = new(this, 3);
        }

        protected override string ValidateName(string? name) {
            return string.IsNullOrWhiteSpace(name) ? "State" : name.Replace(" ", "");
        }

        public StateMachine RemoveNode(Node node) {
            Nodes.Remove(node);

            if (node is InitialState) InitialState = null;

            var connections = Connections
                .Where(x => x.From.Container == node || x.To.Container == node)
                .ToArray();

            foreach (var connection in connections) RemoveConnection(connection);

            return this;
        }

        public StateMachine RemoveAllNodes() {
            foreach (var n in nodes) {
                RemoveNode(n);
            }

            return this;
        }

        public StateMachine TryAddNode(Node node) {
            if (node is InitialState instate) {
                if (InitialState != null) return this;
                else InitialState = instate;
            }

            Nodes.Add(node);

            node.Container = this;

            return this;
        }

        public StateMachine TryAddNodes(IEnumerable<Node> nodes) {
            foreach (var n in nodes) {
                TryAddNode(n);
            }

            return this;
        }

        public Connection? TryAddConnection(Connector from, Connector to) => TryAddConnection(new(from, to));

        public Connection? TryAddConnection(Connection c) {
            var from = c.From;
            var to = c.To;

            if (from.Container == null
                || to.Container == null
                || Connections.Contains(c)
                || from.Container == to.Container) return null;

            // disallow connection TO initial state
            if (to.Container is InitialState) return null;
            // disallow multiple connections from initial state
            if (from.Container is InitialState fromIns && fromIns.Connector.IsConnected == true) return null;
            // disallow connection from exit node
            if (from.Container is ExitNode) return null;

            from.Container.ReactToConnectionAdded(ConnectionSource.From, c);
            to.Container.ReactToConnectionAdded(ConnectionSource.To, c);

            Connections.Add(c);
            from.Connections++;
            to.Connections++;

            c.Container = this;

            return c;
        }

        public StateMachine TryAddConnections(IEnumerable<Connection> connections) {
            foreach (var c in connections) {
                TryAddConnection(c);
            }

            return this;
        }

        public StateMachine RemoveConnection(Connection c) {
            Connections.Remove(c);
            c.From.Connections--;
            c.To.Connections--;

            c.From.Container?.ReactToConnectionRemoved();
            c.To.Container?.ReactToConnectionRemoved();

            return this;
        }

        public StateMachine RemoveAllConnections() {
            foreach (var c in Connections) {
                RemoveConnection(c);
            }

            return this;
        }

        partial void OnIsExpandedChanged(bool value) {
            if (!value) Size = new(0, 0);
            else Size = DesiredSize;
        }
    }
}
