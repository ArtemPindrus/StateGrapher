using CommunityToolkit.Mvvm.ComponentModel;

namespace StateGrapher.Models {
    public partial class InitialState : Node {
        [ObservableProperty]
        private Connector connector;

        public Connection Connection { get; set; }

        public InitialState() {
            Connector = new Connector(this);
        }

        protected override string? ValidateName(string? name) => "InitialState";

        public override void ReactToConnectionAdded(ConnectionSource connectionSource, Connection connection) {
            if (connectionSource != ConnectionSource.From) return;

            Connection = connection;
        }

        public override void ReactToConnectionRemoved() {
            Connection = null;
        }
    }
}
