using StateGrapher.Models;
using System.Windows;

namespace StateGrapher.ViewModels {
    public class NodeViewModel : ViewModelBase {
        public Node Node { get; }

        public Point Location {
            get => Node.Location;
            set => Node.Location = value;
        }

        public NodeViewModel(Node node) {
            Node = node;
        }
    }
}
