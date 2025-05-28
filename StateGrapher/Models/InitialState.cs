using CommunityToolkit.Mvvm.ComponentModel;

namespace StateGrapher.Models {
    public partial class InitialState : Node {
        [ObservableProperty]
        private Connector connector;

        public InitialState() {
            Connector = new Connector(this);
        }

        protected override string? ValidateName(string? name) => "InitialState";
    }
}
