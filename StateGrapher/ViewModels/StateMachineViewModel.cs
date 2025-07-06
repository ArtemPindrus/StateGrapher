using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nodify;
using StateGrapher.Models;
using StateGrapher.Utilities;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace StateGrapher.ViewModels
{
    public partial class StateMachineViewModel : NodeViewModel {
        [ObservableProperty]
        private NodeViewModel? selectedNode;

        [ObservableProperty]
        private NodeViewModel? selectedConnection;

        [ObservableProperty]
        public bool toHightlight;

        public new StateMachine Node { get; }

        public ConnectorsCollectionViewModel Connectors { get; }

        public string? Name {
            get => Node.Name;
            set => Node.Name = value;
        }

        public Size Size {
            get => Node.Size;
            set => Node.Size = value;
        }

        public bool IsExpanded {
            get => Node.IsExpanded;
            set => Node.IsExpanded = value;
        }

        // TODO: why dependency property?

        public static readonly DependencyProperty NodesProperty =
            DependencyProperty.Register(
                "Nodes",
                typeof(ObservableCollection<NodeViewModel>),
                typeof(StateMachineViewModel),
                new PropertyMetadata(null));

        public ObservableCollection<NodeViewModel> Nodes {
            get => (ObservableCollection<NodeViewModel>)GetValue(NodesProperty);
            set => SetValue(NodesProperty, value);
        }

        public ObservableCollection<ConnectionViewModel> Connections { get; }

        public StateMachineViewModel(StateMachine stateMachine) : base(stateMachine) {
            Nodes = new();
            Connections = new();

            Node = stateMachine;

            stateMachine.PropertyChanged += (object? sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
            stateMachine.PropertyChanging += (object? sender, PropertyChangingEventArgs e) => OnPropertyChanging(e);
            stateMachine.Nodes.CollectionChanged += StateMachineNodes_CollectionChanged;
            stateMachine.Connections.CollectionChanged += StateMachineConnections_CollectionChanged;

            foreach (var node in stateMachine.Nodes) TryAddNodeViewModel(node);
            foreach (var connection in stateMachine.Connections) Connections.Add(new ConnectionViewModel(connection));

            Connectors = new(stateMachine.Connectors);
        }

        private void StateMachineConnections_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                if (e.NewItems is null) return;

                foreach (var item in e.NewItems.Cast<Models.Connection>()) {
                    Connections.Add(new(item));
                }
            } else if (e.Action == NotifyCollectionChangedAction.Remove) {
                if (e.OldItems is null) return;

                foreach (var connection in e.OldItems.Cast<Models.Connection>()) {
                    RemoveConnectionViewModel(connection);
                }
            }
        }

        private void StateMachineNodes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                if (e.NewItems is null) return;

                foreach (var node in e.NewItems.Cast<Models.Node>()) TryAddNodeViewModel(node);
            } else if (e.Action == NotifyCollectionChangedAction.Remove) {
                if (e.OldItems is null) return;

                foreach(var node in e.OldItems.Cast<Models.Node>()) {
                    if (Node.Nodes.Contains(node)) return;

                    RemoveNodeViewModel(node);
                }
            }
        }

        private void TryAddNodeViewModel(Models.Node node) {
            if (node is StickyNode sn) Nodes.Add(new StickyNodeViewModel(sn));
            else if (node is StateMachine sm) Nodes.Add(new StateMachineViewModel(sm));
            else if (node is InitialState instate) Nodes.Add(new InitialStateViewModel(instate));
            else if (node is ExitNode en) Nodes.Add(new ExitNodeViewModel(en));

            else throw new ArgumentException($"Node type is not supported. ({node.GetType().Name})");
        }

        private void RemoveNodeViewModel(Models.Node node) {
            var nodeViewModel = Nodes.First(x => x.Node == node);
            Nodes.Remove(nodeViewModel);
        }

        private void RemoveConnectionViewModel(Models.Connection connection) {
            Connections.Remove(Connections.First(x => x.Connection == connection));
        }

        [RelayCommand]
        private void AddConnection(object o) {
            if (o is not ValueTuple<object, object> connectors
                || connectors.Item1 is not ConnectorViewModel from
                || connectors.Item2 is not ConnectorViewModel to
                || from == to) return;

            Node.TryAddConnection(from.Connector, to.Connector);
        }

        [RelayCommand]
        public void DeleteConnection(ConnectionViewModel connection) {
            Node.RemoveConnection(connection.Connection);
        }

        [RelayCommand]
        private void DeleteNode(NodeViewModel nodeVM) {
            Node.RemoveNode(nodeVM.Node);
        }

        [RelayCommand]
        private void CreateStateMachineNode(Point location) {
            StateMachine state = new() { Location = location };
            Node.TryAddNode(state);
        }

        [RelayCommand]
        private void CreateStickyNode(Point location) {
            StickyNode sn = new() { Location = location };
            Node.TryAddNode(sn);
        }

        [RelayCommand]
        private void CreateInitialStateNode(Point location) {
            InitialState node = new() { Location = location };
            Node.TryAddNode(node);
        }

        [RelayCommand]
        private void CreateExitNode(Point location) {
            ExitNode node = new() { Location = location };
            Node.TryAddNode(node);
        }

        partial void OnSelectedNodeChanged(NodeViewModel? value) {
            History.LastSelectedNode = value;
        }

        partial void OnSelectedConnectionChanged(NodeViewModel? value) {
            History.LastSelectedConnection = value;
        }
    }
}
