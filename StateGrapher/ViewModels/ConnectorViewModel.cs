using StateGrapher.Models;
using System.Windows;

namespace StateGrapher.ViewModels
{
    public partial class ConnectorViewModel : ViewModelBase {
        public Connector Connector { get; }

        public Point Anchor {
            get => Connector.Anchor;
            set => Connector.Anchor = value;
        }

        public bool IsConnected => Connector.IsConnected;

        public ConnectorViewModel(Connector connector) {
            this.Connector = connector;

            connector.PropertyChanged += (_, e) => OnPropertyChanged(e);
        }
    }
}
