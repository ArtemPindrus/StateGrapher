
using System.Runtime.CompilerServices;

namespace StateGrapher.Models {
    public partial class InitialState : Node, IConnectingNode {
        public Connector Connector { get; }

        public Connection? Connection { get; private set; }

        public InitialState(StateMachine container) : base(container) {
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

        public IEnumerable<Connector> GetAllConnectors() {
            yield return Connector;
        }
    }
}
