using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nodify;
using StateGrapher.Models;
using StateGrapher.Utilities;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Permissions;
using System.Windows;

namespace StateGrapher.ViewModels {
    public partial class MainViewModel : ViewModelBase {
        [ObservableProperty]
        private StateMachineViewModel firstOrderStateMachineViewModel;

        public string LastActionHint => History.LastActionHint;

        public MainViewModel() {
            StateMachine firstOrderSM = new StateMachine() { Name = "FirstOrder", IsExpanded = true };

            StateMachineViewModel vm = new(firstOrderSM);
            FirstOrderStateMachineViewModel = vm;


            History.PropertyChanged += (_, e) => OnPropertyChanged(e);
        }

        [RelayCommand]
        private void SaveToLast() {
            if (!GraphSerializer.SerializeToLast(FirstOrderStateMachineViewModel.StateMachine)) { 
                SaveToFile();    
            }
        }

        [RelayCommand]
        private void SaveToFile() {
            string? directoryPath = FolderDialog.RequestSaveGraphPath();

            if (directoryPath is null) return;

            GraphSerializer.SerializeToFile(directoryPath, FirstOrderStateMachineViewModel.StateMachine);
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
