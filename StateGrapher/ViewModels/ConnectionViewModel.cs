using CommunityToolkit.Mvvm.Input;
using StateGrapher.Models;
using System.Collections.ObjectModel;
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

        public ObservableCollection<ConnectionCondition> ForwardConditions => Connection.ForwardConditions;
        public ObservableCollection<ConnectionCondition> BackwardsConditions => Connection.BackwardsConditions;

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
        private void AddForwardCondition(StateMachineBool boolean) {
            if (boolean == null
                || Connection.ForwardConditions.Any(x => x.SmBoolean == boolean)) return;

            Connection.ForwardConditions.Add(new(false, boolean));
        }

        [RelayCommand]
        private void AddBackwardsCondition(StateMachineBool boolean) {
            if (boolean == null
                || Connection.BackwardsConditions.Any(x => x.SmBoolean == boolean)) return;

            Connection.BackwardsConditions.Add(new(false, boolean));
        }

        public void RemoveCondition(StateMachineBool boolean) {
            Connection.RemoveCondition(boolean);
        }

            [RelayCommand]
        private void Remove() {
            machineViewModel.DeleteConnection(this);
        }
    }
}
