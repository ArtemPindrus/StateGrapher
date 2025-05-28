using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace StateGrapher.Models
{
    public partial class StateMachine : Node {
        [ObservableProperty]
        private bool isExpanded;

        private InitialState? initialState;

        public ObservableCollection<Node> Nodes { get; set; } = new();
        public ObservableCollection<Connection> Connections { get; set; } = new();

        public Connector LeftConnector { get; set; }
        public Connector TopConnector { get; set; }
        public Connector RightConnector { get; set; }
        public Connector BottomConnector { get; set; }

        public StateMachine() {
            LeftConnector = new(this);
            TopConnector = new(this);
            RightConnector = new(this);
            BottomConnector = new(this);
        }

        protected override string ValidateName(string? name) {
            return string.IsNullOrWhiteSpace(name) ? "State" : name.Replace(" ", "");
        }

        public StateMachine RemoveNode(Node node) {
            Nodes.Remove(node);

            if (node is InitialState) initialState = null;

            var connections = Connections
                .Where(x => x.From.Container == node || x.To.Container == node)
                .ToArray();

            foreach (var connection in connections) RemoveConnection(connection);

            return this;
        }

        public StateMachine TryAddNode(Node node) {
            if (node is InitialState instate) {
                if (initialState != null) return this;
                else initialState = instate;
            }

            Nodes.Add(node);
            return this;
        }

        public Connection? TryAddConnection(Connector from, Connector to) {
            Connection c = new(from, to);

            if (Connections.Contains(c)
                || from.Container == to.Container
                || Connections.Any(x => (x.From.Container == from.Container && x.To.Container == to.Container)
                || (x.From.Container == to.Container && x.To.Container == from.Container))) return null;

            // disallow connection TO initial state
            if (to.Container is InitialState) return null;
            // disallow multiple connections from initial state
            if (from.Container is InitialState fromIns && fromIns.Connector.IsConnected == true) return null;
            // disallow connection from exit node
            if (from.Container is ExitNode) return null;

            Connections.Add(c);
            from.Connections++;
            to.Connections++;

            return c;
        }

        public void RemoveConnection(Connection c) {
            Connections.Remove(c);
            c.From.Connections--;
            c.To.Connections--;
        }

        partial void OnIsExpandedChanged(bool value) {
            if (!value) Size = new(0, 0);
            else Size = DesiredSize;
        }
    }
}
