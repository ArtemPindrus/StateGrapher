using CommunityToolkit.Mvvm.ComponentModel;
using Mapster;
using StateGrapher.Extensions;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace StateGrapher.Models
{
    [AdaptTo("[name]DTO")]
    public partial class Options : ObservableObject {
        private const string DefaultClassName = "StateMachine";

        [ObservableProperty]
        private string className = DefaultClassName;

        [ObservableProperty]
        private string? namespaceName;

        public ObservableCollection<StateMachineBool> StateMachineBooleans { get; }

        public string ClassFullName {
            get {
                if (!string.IsNullOrWhiteSpace(NamespaceName)) return $"{NamespaceName}.{ClassName}";
                else return ClassName;
            }
        }

        public Options(ObservableCollection<StateMachineBool>? stateMachineBooleans = null) {
            StateMachineBooleans = stateMachineBooleans ?? new();
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
