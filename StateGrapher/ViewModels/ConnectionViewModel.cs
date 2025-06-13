using CommunityToolkit.Mvvm.Input;
using StateGrapher.Models;
using System.Windows;
using System.Windows.Controls;
using ConnectionDirection = Nodify.ConnectionDirection;

namespace StateGrapher.ViewModels
{
    public partial class ConnectionViewModel : ViewModelBase, INodeViewModel<Connection> {
        private readonly StateMachineViewModel machineViewModel;

        public string? Name {
            get => Connection.Name;
            set => Connection.Name = value;
        }

        public string? EventDisplayName => Connection.EventDisplayName;

        public string? ForwardEvent {
            get => Connection.ForwardEvent;
            set => Connection.ForwardEvent = value;
        }

        public string? BackEvent {
            get => Connection.BackEvent;
            set => Connection.BackEvent = value;
        }

        public bool IsBothWays {
            get => Connection.IsBothWays;
            set => Connection.IsBothWays = value;
        }

        public ConnectionDirection Direction { 
            get => Connection.Direction;
            set => Connection.Direction = value;
        }

        public Orientation SourceOrientation {
            get => Connection.SourceOrientation;
            set => Connection.SourceOrientation = value;
        }

        public Orientation TargetOrientation {
            get => Connection.TargetOrientation;
            set => Connection.TargetOrientation = value;
        }

        public Connection Connection { get; }
        public Connector From => Connection.From;
        public Connector To => Connection.To;

        public Point Location { get => new(0, 0); set { } }

        Connection INodeViewModel<Connection>.Node => Connection;

        Node INodeViewModel.Node => Connection;

        public bool ToHightlight => false;

        public ConnectionViewModel(Connection connection, StateMachineViewModel machineViewModel) {
            Connection = connection;
            this.machineViewModel = machineViewModel;

            Connection.PropertyChanged += (_, e) => OnPropertyChanged(e);
        }

        [RelayCommand]
        private void Remove() {
            machineViewModel.DeleteConnection(this);
        }
    }
}
