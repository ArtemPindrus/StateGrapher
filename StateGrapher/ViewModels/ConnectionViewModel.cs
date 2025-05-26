using CommunityToolkit.Mvvm.Input;
using StateGrapher.Models;
using StateGrapher.ViewModels;

namespace StateGrapher.ViewModels
{
    public partial class ConnectionViewModel : ViewModelBase {
        private readonly StateMachineViewModel machineViewModel;

        public Connection Connection { get; }
        public Connector From => Connection.From;
        public Connector To => Connection.To;

        public ConnectionViewModel(Connection connection, StateMachineViewModel machineViewModel) {
            Connection = connection;
            this.machineViewModel = machineViewModel;
        }

        [RelayCommand]
        private void Remove() {
            machineViewModel.DeleteConnection(this);
        }
    }
}
