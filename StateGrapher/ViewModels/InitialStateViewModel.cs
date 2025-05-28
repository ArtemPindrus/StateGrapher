using StateGrapher.Models;
using System.Windows;

namespace StateGrapher.ViewModels
{
    public class InitialStateViewModel : ViewModelBase, INodeViewModel<InitialState> {
        public InitialState Node { get; set; }

        public Point Location { 
            get => Node.Location; 
            set => Node.Location = value; 
            }


        public ConnectorViewModel Connector { get; }

        InitialState INodeViewModel<InitialState>.Node => Node;
        Node INodeViewModel.Node => Node;

        public InitialStateViewModel(InitialState node) {
            Node = node;

            Connector = new(Node.Connector);
        }
    }
}
