using StateGrapher.Models;

namespace StateGrapher.ViewModels
{
    public class ConnectorsCollectionViewModel : ViewModelBase {
        private readonly ConnectorsCollection connectors;

        public ConnectorViewModel[] LeftConnectors { get; }
        public ConnectorViewModel[] TopConnectors { get; }
        public ConnectorViewModel[] RightConnectors { get; }
        public ConnectorViewModel[] BottomConnectors { get; }

        public ConnectorsCollectionViewModel(ConnectorsCollection connectors) {
            this.connectors = connectors;

            LeftConnectors = new ConnectorViewModel[connectors.LeftConnectors.Length];
            TopConnectors = new ConnectorViewModel[connectors.TopConnectors.Length];
            RightConnectors = new ConnectorViewModel[connectors.RightConnectors.Length];
            BottomConnectors = new ConnectorViewModel[connectors.BottomConnectors.Length];

            Init(connectors.LeftConnectors, LeftConnectors);
            Init(connectors.TopConnectors, TopConnectors);
            Init(connectors.RightConnectors, RightConnectors);
            Init(connectors.BottomConnectors, BottomConnectors);
        }

        public static void Init(Connector[] connectors, ConnectorViewModel[] viewModels) {
            for (int i = 0; i < connectors.Length; i++) {
                viewModels[i] = new ConnectorViewModel(connectors[i]);
            }
        }
    }
}
