using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nodify;
using StateGrapher.Models;
using StateGrapher.Utilities;

namespace StateGrapher.ViewModels {
    public partial class MainViewModel : ViewModelBase {
        [ObservableProperty]
        private StateMachineViewModel firstOrderStateMachineViewModel;

        public string? LastActionHint => History.LastActionHint;

        public MainViewModel() {
            StateMachine firstOrderSM = new() { Name = "FirstOrder", IsExpanded = true };

            StateMachineViewModel vm = new(firstOrderSM);
            FirstOrderStateMachineViewModel = vm;

            //SAMPLE
            StateMachine f = new() { Name = "First" };
            StateMachine s = new() { Name = "Second", Location = new(500, 0) };

            firstOrderSM.AddNode(f).AddNode(s).TryAddConnection(f.BottomConnector, s.BottomConnector);

            History.PropertyChanged += (_, e) => OnPropertyChanged(e);
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
