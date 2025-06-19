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
        private StateMachineViewModel rootStateMachineViewModel;

        [ObservableProperty]
        private OptionsViewModel optionsViewModel;
        
        [ObservableProperty]
        private TestingEnvironment? testingEnvironment;

        [ObservableProperty]
        private StateMachineViewModel? currentTestState;

        [ObservableProperty]
        private Transition[] transitions;

        public int DispatchEventID { get; set; }

        public string? LastActionHint => History.LastActionHint;

        public MainViewModel() {
            NewGraph();

            History.PropertyChanged += (_, e) => OnPropertyChanged(e);
        }

        [RelayCommand]
        private void RefreshTransitions() {
            Transitions = StateMachineUtility.ToTransitions(RootStateMachineViewModel.Node.GetHierarchyConnections())
                .ToArray();
        }

        [RelayCommand]
        private void NewGraph() {
            StateMachine root = new() { Name = "ROOT", IsExpanded = true };
            StateMachineViewModel vm = new(root);
            RootStateMachineViewModel = vm;

            Options op = new();
            OptionsViewModel = new(op);

            TestingEnvironment = null;
            CurrentTestState = null;
        }

        [RelayCommand]
        private void CreateTestingEnvironment() {
            string classString = StateMachineClassGenerator.GenerateCSharpClass(new(RootStateMachineViewModel.Node, OptionsViewModel.Options));

            TestingEnvironment = TestingEnvironment.FromGeneratedClass(classString);

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
        private void Load() { 
            string? filePath = FolderDialog.RequestGraphPath();

            if (filePath is null) return;

            GraphSerializer.DeserializeFromFile(filePath, out Graph? graph);

            if (graph is not Graph validGraph
                || validGraph.RootStateMachine == null) return;

            RootStateMachineViewModel = new(validGraph.RootStateMachine);
            OptionsViewModel = new(validGraph.Options);
        }

        partial void OnCurrentTestStateChanged(StateMachineViewModel? oldValue, StateMachineViewModel? newValue) {
            if (oldValue != null) oldValue.ToHightlight = false;
            if (newValue != null) newValue.ToHightlight = true;
        }
    }
}
