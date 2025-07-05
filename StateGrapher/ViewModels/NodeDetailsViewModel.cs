using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StateGrapher.Models;
using System.Collections.ObjectModel;

namespace StateGrapher.ViewModels
{
    public partial class NodeDetailsViewModel : ViewModelBase {
        [ObservableProperty]
        public StateMachineBool? selectedBoolean;
    }
}
