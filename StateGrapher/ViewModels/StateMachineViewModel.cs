using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StateGrapher.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace StateGrapher.ViewModels
{
    public partial class StateMachineViewModel : ViewModelBase {
        public StateMachine StateMachine { get; }

        public string? Name {
            get => StateMachine.Name;
            set => StateMachine.Name = value;
        }

        public Point Location {
            get => StateMachine.Location;
            set => StateMachine.Location = value;
        }

        public Size Size {
            get => StateMachine.Size;
            set => StateMachine.Size = value;
        }

        public bool IsExpanded {
            get => StateMachine.IsExpanded;
            set => StateMachine.IsExpanded = value;
        }

        public static readonly DependencyProperty NodesProperty =
            DependencyProperty.Register(
                "Nodes",
                typeof(ObservableCollection<ViewModelBase>),
                typeof(StateMachineViewModel),
                new PropertyMetadata(null));

        public ObservableCollection<ViewModelBase> Nodes {
            get => (ObservableCollection<ViewModelBase>)GetValue(NodesProperty);
            set => SetValue(NodesProperty, value);
        }

        public StateMachineViewModel(StateMachine stateMachine) {
            Nodes = new();
            this.StateMachine = stateMachine;

            stateMachine.PropertyChanged += (object? sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
            stateMachine.PropertyChanging += (object? sender, PropertyChangingEventArgs e) => OnPropertyChanging(e);


            foreach (var node in stateMachine.Nodes) AddNode(node, false);
        }

        private void AddNode(Node node, bool stateMachineInclusion = true) {
            if (stateMachineInclusion) StateMachine.Nodes.Add(node);

            if (node is StickyNode sn) Nodes.Add(new StickyNodeViewModel(sn));
            else if (node is StateMachine sm) Nodes.Add(new StateMachineViewModel(sm));
        }

        private void RemoveNodeAt(int i) {
            StateMachine.Nodes.RemoveAt(i);
            Nodes.RemoveAt(i);
        }

        [RelayCommand]
        private void DeleteNode(ViewModelBase node) {
            int i = Nodes.IndexOf(node);

            RemoveNodeAt(i);
        }

        [RelayCommand]
        private void CreateStateMachineNode(Point location) {
            StateMachine state = new() { Location = location };
            AddNode(state);
        }

        [RelayCommand]
        private void CreateStickyNode(Point location) {
            StickyNode sn = new() { Location = location };
            AddNode(sn);
        }
    }
}
