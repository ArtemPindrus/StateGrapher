using StateGrapher.Models;
using System.Collections.ObjectModel;

namespace StateGrapher.ViewModels {
    public class OptionsViewModel : ViewModelBase {
        public readonly Options Options;

        public string ClassName { 
            get => Options.ClassName; 
            set => Options.ClassName = value;
        }

        public string? Namespace { 
            get => Options.NamespaceName;
            set => Options.NamespaceName = value;
        }

        public ObservableCollection<StateMachineBool> StateMachineBooleans => Options.StateMachineBooleans;

        public string ClassFullName => Options.ClassFullName;

        public OptionsViewModel(Options options) {
            this.Options = options;

            Options.PropertyChanged += (o, e) => OnPropertyChanged(e);
        }
    }
}
