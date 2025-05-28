using StateGrapher.Models;
using System.Windows;

namespace StateGrapher.ViewModels
{
    public class ExitNodeViewModel : ViewModelBase, INodeViewModel<ExitNode> {
        private readonly ExitNode exitNode;

        public ExitNode Node => exitNode;
        Node INodeViewModel.Node => Node;

        public ConnectorViewModel Connector { get; }

        public string? Name {
            get => exitNode.Name;
            set => exitNode.Name = value;
        }

        public Point Location {
            get => exitNode.Location; 
            set => exitNode.Location = value;
        }


        public ExitNodeViewModel(ExitNode exitNode) {
            this.exitNode = exitNode;
            Connector = new(exitNode.Connector);

            exitNode.PropertyChanged += (_, e) => OnPropertyChanged(e);
        }
    }
}
