using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.CodeAnalysis;
using StateGrapher.Extensions;
using StateGrapher.Models;
using StateGrapher.Testing;
using StateGrapher.Utilities;
using StateGrapher.Views;
using System.IO;

namespace StateGrapher.ViewModels {
    public partial class MainViewModel : ViewModelBase {
        [ObservableProperty]
        private StateMachineViewModel? rootStateMachineViewModel;

        [ObservableProperty]
        private OptionsViewModel? optionsViewModel;

        [ObservableProperty]
        private TestingViewModel? testingViewModel;

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
            else if (e.PropertyName == nameof(App.TestingEnvironment)) OnNewTestingEnvironmentCreated(App.TestingEnvironment);
        }

        private void OnNewGraphCreated(Graph graph) {
            RootStateMachineViewModel = new(graph.RootStateMachine);
            OptionsViewModel = new(graph.Options);

            TestingViewModel = null;
            CurrentTestState = null;
            Transitions = null;
        }

        private void OnNewTestingEnvironmentCreated(TestingEnvironment? testing) {
            if (testing == null) {
                TestingViewModel = null;
            } else {
                TestingViewModel = new(testing);

                UpdateCurrentTestState(TestingViewModel.CurrentState);
            }
        }

        [RelayCommand]
        private void RefreshTransitions() {
            Transitions = StateMachineUtility.ToTransitions(RootStateMachineViewModel.Node.GetHierarchyConnections())
                .ToArray();
        }

        [RelayCommand]
        private void NewGraph() => App.CreateNewGraph();

        [RelayCommand]
        private void RefreshTestingEnvironment() => App.RefreshTestingEnvironment();

        [RelayCommand]
        private void DispatchTestingEvent() {
            if (TestingViewModel == null) return;

            TestingViewModel.DispatchTestingEvent(DispatchEventID);

            UpdateCurrentTestState(TestingViewModel.CurrentState);
        }

        private void UpdateCurrentTestState(string stateName) {
            CurrentTestState = RootStateMachineViewModel
                .GetHierarchyNodes().FirstOrDefault((Func<StateMachineViewModel, bool>)(x => x.Name == stateName));
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
