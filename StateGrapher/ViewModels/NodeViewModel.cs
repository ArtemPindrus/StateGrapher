using StateGrapher.Models;
using System.Windows;

namespace StateGrapher.ViewModels {
    public class NodeViewModel : ViewModelBase {
        public Node Node { get; }

        public Point Location {
            get => Node.Location;
            set => Node.Location = value;
        }

        public string? Name {
            get => Node.Name;
            set => Node.Name = value;
        }

        public Size Size {
            get => Node.Size;
            set => Node.Size = value;
        }

        public NodeViewModel(Node node) {
            Node = node;
        }
    }
}
