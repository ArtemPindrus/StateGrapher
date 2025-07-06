using StateGrapher.Models;
using System.Windows;

namespace StateGrapher.ViewModels
{
    public class ExitNodeViewModel : NodeViewModel {
        public ConnectorViewModel Connector { get; }

        public ExitNodeViewModel(ExitNode exitNode) : base(exitNode) {
            Connector = new(exitNode.Connector);

            exitNode.PropertyChanged += (_, e) => OnPropertyChanged(e);
        }
    }
}
