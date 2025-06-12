using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StateGrapher.Extensions;
using StateGrapher.Models;
using StateGrapher.Utilities;

namespace StateGrapher.ViewModels {
    public partial class MainViewModel : ViewModelBase {
        // TODO: rename to root lol
        [ObservableProperty]
        private StateMachineViewModel firstOrderStateMachineViewModel;

        public string? LastActionHint => History.LastActionHint;

        public MainViewModel() {
            StateMachine firstOrderSM = new() { Name = "ROOT", IsExpanded = true };

            StateMachineViewModel vm = new(firstOrderSM);
            FirstOrderStateMachineViewModel = vm;

            //SAMPLE
            //StateMachine f = new() { Name = "First" };
            //StateMachine s = new() { Name = "Second", Location = new(500, 0) };

            //firstOrderSM.TryAddNode(f).TryAddNode(s).TryAddConnection(f.Connectors.TopConnectors[0], s.Connectors.TopConnectors[0]);

            //GraphSerializer.DeserializeFromFile(@"C:\Users\Artem\Downloads\CrouchingGraph.json", out var env);

            //FirstOrderStateMachineViewModel = new(env.FirstOrderStateMachine);

            //StateMachineParser.GenerateCSharpClass(FirstOrderStateMachineViewModel.Node);

            History.PropertyChanged += (_, e) => OnPropertyChanged(e);
        }

        [RelayCommand]
        private void TestParse() {
            StateMachineClassGenerator.GenerateCSharpClass(firstOrderStateMachineViewModel.Node);
        }

        [RelayCommand]
        private void SaveToLast() {
            if (!GraphSerializer.SerializeToLast(FirstOrderStateMachineViewModel.Node)) { 
                SaveToFile();    
            }
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
    }
}
