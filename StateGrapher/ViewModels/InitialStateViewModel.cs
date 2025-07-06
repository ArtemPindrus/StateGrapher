using StateGrapher.Models;
using System.Windows;

namespace StateGrapher.ViewModels
{
    public class InitialStateViewModel : NodeViewModel {
        public ConnectorViewModel Connector { get; }

        public InitialStateViewModel(InitialState node) : base(node) {
            Connector = new(node.Connector);
        }
    }
}
