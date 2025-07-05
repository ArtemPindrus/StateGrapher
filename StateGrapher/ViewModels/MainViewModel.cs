using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.CodeAnalysis;
using StateGrapher.Extensions;
using StateGrapher.Models;
using StateGrapher.Utilities;
using System.IO;

namespace StateGrapher.ViewModels {
    public partial class MainViewModel : ViewModelBase {
        [ObservableProperty]
        private StateMachineViewModel? rootStateMachineViewModel;

        [ObservableProperty]
        private OptionsViewModel? optionsViewModel;

        [ObservableProperty]
        private TestingEnvironment? testingEnvironment;

        [ObservableProperty]
        private StateMachineViewModel? currentTestState;

        [ObservableProperty]
        private Transition[]? transitions;

        public int DispatchEventID { get; set; }

        public string? LastActionHint => History.LastActionHint;

        public MainViewModel(Graph graph) {
            History.PropertyChanged += (_, e) => OnPropertyChanged(e);
            App.StaticPropertyChanged += App_StaticPropertyChanged;

            OnNewGraphCreated(graph);
        }

        private void App_StaticPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(App.CurrentGraph)) OnNewGraphCreated(App.CurrentGraph);
        }

        private void OnNewGraphCreated(Graph graph) {
            RootStateMachineViewModel = new(graph.RootStateMachine);
            OptionsViewModel = new(graph.Options);

            TestingEnvironment = null;
            CurrentTestState = null;
        }

        [RelayCommand]
        private void RefreshTransitions() {
            Transitions = StateMachineUtility.ToTransitions(RootStateMachineViewModel.Node.GetHierarchyConnections())
                .ToArray();
        }

        [RelayCommand]
        private void NewGraph() => App.CreateNewGraph();

        [RelayCommand]
        private void CreateTestingEnvironment() {
            string classString = StateMachineClassGenerator.GenerateCSharpClass(new(RootStateMachineViewModel.Node, OptionsViewModel.Options));

            TestingEnvironment = TestingEnvironment.FromGeneratedClass(classString, OptionsViewModel.ClassFullName);

            if (TestingEnvironment == null) return;

            CurrentTestState = RootStateMachineViewModel
                .GetHierarchyNodes().FirstOrDefault(x => x.Name == TestingEnvironment.CurrentState);
        }

        [RelayCommand]
        private void DispatchTestingEvent() {
            if (TestingEnvironment == null) return;

            TestingEnvironment.DispatchEvent(DispatchEventID);

            CurrentTestState = RootStateMachineViewModel
                .GetHierarchyNodes().FirstOrDefault(x => x.Name == TestingEnvironment.CurrentState);
        }

        [RelayCommand]
        private void SaveToLast() {
            if (!GraphSerializer.SerializeToLast(new(RootStateMachineViewModel.Node, OptionsViewModel.Options))) { 
                SaveToFile();    
            }
        }

        [RelayCommand]
        private void GenerateCSharpClass() {
            string? path = FolderDialog.RequestSaveCSharpClassPath();

            if (path == null) return;

            var classString = StateMachineClassGenerator.GenerateCSharpClass(new(RootStateMachineViewModel.Node, OptionsViewModel.Options));
            File.WriteAllText(path, classString);
        }

        [RelayCommand]
        private void SaveToFile() {
            string? directoryPath = FolderDialog.RequestSaveGraphPath();

            if (directoryPath is null) return;

            GraphSerializer.SerializeToFile(directoryPath, new(RootStateMachineViewModel.Node, OptionsViewModel.Options));
        }

        [RelayCommand]
        private void Load() => App.LoadGraph();

        partial void OnCurrentTestStateChanged(StateMachineViewModel? oldValue, StateMachineViewModel? newValue) {
            if (oldValue != null) oldValue.ToHightlight = false;
            if (newValue != null) newValue.ToHightlight = true;
        }
    }
}
