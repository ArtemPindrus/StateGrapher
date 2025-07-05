using CommunityToolkit.Mvvm.ComponentModel;
using StateGrapher.Extensions;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace StateGrapher.Models
{
    public partial class Options : ObservableObject {
        private const string DefaultClassName = "StateMachine";

        [ObservableProperty]
        private string className = DefaultClassName;

        [ObservableProperty]
        private string? namespaceName;
        private ObservableCollection<StateMachineBool> stateMachineBooleans = new();

        public ObservableCollection<StateMachineBool> StateMachineBooleans {
            get => stateMachineBooleans;
            set {
                stateMachineBooleans = value;

                stateMachineBooleans.CollectionChanged += OnStateMachineBooleansChanged;
            }
        }

        public string ClassFullName {
            get {
                if (!string.IsNullOrWhiteSpace(NamespaceName)) return $"{NamespaceName}.{ClassName}";
                else return ClassName;
            }
        }

        partial void OnClassNameChanged(string value) {
            if (string.IsNullOrWhiteSpace(value)) ClassName = DefaultClassName;
        }

        private void OnStateMachineBooleansChanged(object? sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Remove) {
                if (e.OldItems == null) return;

                var connections = App.CurrentGraph.RootStateMachine
                    .GetHierarchyConnections()
                    .ToArray();

                foreach (StateMachineBool b in e.OldItems) {
                    foreach (var c in connections) {
                        c.RemoveCondition(b);
                    }
                }
            }
        }
    }
}
