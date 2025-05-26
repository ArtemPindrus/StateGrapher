using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace StateGrapher.Models
{
    public partial class StateMachine : Node {
        [ObservableProperty]
        private bool isExpanded;

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
            return this;
        }

        public StateMachine AddNode(Node node) {
            Nodes.Add(node); 
            return this;
        }

        public Connection? TryAddConnection(Connector from, Connector to) {
            Connection c = new(from, to);

            if (Connections.Contains(c)
                || from.Container == to.Container
                || Connections.Any(x => x.From.Container == from.Container && x.To.Container == to.Container)) return null;

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
