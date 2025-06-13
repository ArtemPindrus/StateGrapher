using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Nodify;
using StateGrapher.Extensions;
using StateGrapher.Models;
using StateGrapher.Utilities;
using System.IO;
using System.Runtime.Loader;

namespace StateGrapher.ViewModels {
    public partial class MainViewModel : ViewModelBase {
        // TODO: rename to root lol
        [ObservableProperty]
        private StateMachineViewModel firstOrderStateMachineViewModel;
        
        [ObservableProperty]
        private TestingEnvironment? testingEnvironment;

        [ObservableProperty]
        private StateMachineViewModel? currentTestState;

        public int DispatchEventID { get; set; }

        public string? LastActionHint => History.LastActionHint;

        public MainViewModel() {
            StateMachine firstOrderSM = new() { Name = "ROOT", IsExpanded = true };

            StateMachineViewModel vm = new(firstOrderSM);
            FirstOrderStateMachineViewModel = vm;

            History.PropertyChanged += (_, e) => OnPropertyChanged(e);
        }

        [RelayCommand]
        private void CreateTestingEnvironment() {
            string classString = StateMachineClassGenerator.GenerateCSharpClass(FirstOrderStateMachineViewModel.Node);

            TestingEnvironment = TestingEnvironment.FromGeneratedClass(classString);

            if (TestingEnvironment == null) return;

            CurrentTestState = FirstOrderStateMachineViewModel
                .GetHierarchyNodes().FirstOrDefault(x => x.Name == TestingEnvironment.CurrentState);
        }

        [RelayCommand]
        private void DispatchTestingEvent() {
            if (TestingEnvironment == null) return;

            TestingEnvironment.DispatchEvent(DispatchEventID);

            CurrentTestState = FirstOrderStateMachineViewModel
                .GetHierarchyNodes().FirstOrDefault(x => x.Name == TestingEnvironment.CurrentState);
        }

        [RelayCommand]
        private void SaveToLast() {
            if (!GraphSerializer.SerializeToLast(FirstOrderStateMachineViewModel.Node)) { 
                SaveToFile();    
            }
        }

        [RelayCommand]
        private void GenerateCSharpClass() {
            string? path = FolderDialog.RequestSaveCSharpClassPath();

            if (path == null) return;

            var classString = StateMachineClassGenerator.GenerateCSharpClass(FirstOrderStateMachineViewModel.Node);
            File.WriteAllText(path, classString);
        }

        [RelayCommand]
        private void SaveToFile() {
            string? directoryPath = FolderDialog.RequestSaveGraphPath();

            if (directoryPath is null) return;

            GraphSerializer.SerializeToFile(directoryPath, FirstOrderStateMachineViewModel.Node);
        }

        [RelayCommand]
        private void Load() { 
            string? filePath = FolderDialog.RequestGraphPath();

            if (filePath is null) return;

            GraphSerializer.DeserializeFromFile(filePath, out Utilities.Environment env);

            FirstOrderStateMachineViewModel = new(env.FirstOrderStateMachine);
        }

        partial void OnCurrentTestStateChanged(StateMachineViewModel? oldValue, StateMachineViewModel? newValue) {
            if (oldValue != null) oldValue.ToHightlight = false;
            if (newValue != null) newValue.ToHightlight = true;
        }
    }
}
