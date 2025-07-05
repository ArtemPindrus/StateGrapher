using CommunityToolkit.Mvvm.ComponentModel;
using Mapster;
using System.Collections.ObjectModel;

namespace StateGrapher.Models
{
    [AdaptTo("[name]DTO")]
    public partial class StateMachine : Node {
        [ObservableProperty]
        private bool isExpanded;

        public InitialState? InitialState { get; set; }

        public ObservableCollection<Node> Nodes { get; set; } = new();

        public ObservableCollection<Connection> Connections { get; set; } = new();

        public ConnectorsCollection Connectors { get; set; }

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

        public StateMachine TryAddNode(Node node) {
            if (node is InitialState instate) {
                if (InitialState != null) return this;
                else InitialState = instate;
            }

            Nodes.Add(node);

            node.Container = this;

            return this;
        }

        public Connection? TryAddConnection(Connector from, Connector to) {
            Connection c = new(from, to);

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

        public void RemoveConnection(Connection c) {
            Connections.Remove(c);
            c.From.Connections--;
            c.To.Connections--;

            c.From.Container?.ReactToConnectionRemoved();
            c.To.Container?.ReactToConnectionRemoved();
        }

        partial void OnIsExpandedChanged(bool value) {
            if (!value) Size = new(0, 0);
            else Size = DesiredSize;
        }
    }
}
