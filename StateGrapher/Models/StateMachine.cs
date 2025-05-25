using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace StateGrapher.Models
{
    public partial class StateMachine : Node {
        [ObservableProperty]
        private bool isExpanded;

        public ObservableCollection<Node> Nodes { get; }

        public StateMachine() {
            Nodes = new();
        }

        [JsonConstructor]
        public StateMachine(ObservableCollection<Node> nodes, bool isExpanded) {
            Nodes = nodes;
            IsExpanded = isExpanded;
        }

        protected override string ValidateName(string? name) {
            return string.IsNullOrWhiteSpace(name) ? "State" : name.Replace(" ", "");
        }

        public StateMachine AddNode(Node node) {
            Nodes.Add(node); 
            return this;
        }

        partial void OnIsExpandedChanged(bool value) {
            if (!value) Size = new(0, 0);
            else Size = DesiredSize;
        }
    }
}
