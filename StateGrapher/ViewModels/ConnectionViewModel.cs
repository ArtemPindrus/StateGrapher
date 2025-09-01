using CommunityToolkit.Mvvm.Input;
using StateGrapher.Models;
using System.Collections.ObjectModel;

namespace StateGrapher.ViewModels
{
    public partial class ConnectionViewModel : ViewModelBase {
        public readonly Connection Connection;

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

        public bool DisplayConditions {
            get => Connection.DisplayConditions;
            set => Connection.DisplayConditions = value;
        }

        public ConnectionCondition? SelectedForwardCondition { get; set; }
        public ConnectionCondition? SelectedBackwardsCondition { get; set; }

        public ObservableCollection<ConnectionCondition> ForwardConditions => Connection.ForwardConditions;
        public ObservableCollection<ConnectionCondition> BackwardsConditions => Connection.BackwardsConditions;

        public StateMachine Container => Connection.Container;

        public Connector? From => Connection.From;
        public Connector? To => Connection.To;

        public ConnectionViewModel(Connection connection) {
            this.Connection = connection;

            this.Connection.PropertyChanged += (_, e) => base.OnPropertyChanged(e);
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

        [RelayCommand]
        private void DeleteForwardCondition(ConnectionCondition c) => Connection.ForwardConditions.Remove(c);

        [RelayCommand]
        private void DeleteBackwardsCondition(ConnectionCondition c) => Connection.BackwardsConditions.Remove(c);

        public void RemoveCondition(StateMachineBool boolean) {
            Connection.RemoveCondition(boolean);
        }

        [RelayCommand]
        private void Remove() {
            Container.RemoveConnection(Connection);
        }
    }
}
