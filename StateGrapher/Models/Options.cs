using CommunityToolkit.Mvvm.ComponentModel;

namespace StateGrapher.Models
{
    public partial class Options : ObservableObject {
        private const string DefaultClassName = "StateMachine";

        [ObservableProperty]
        private string className = DefaultClassName;

        [ObservableProperty]
        private string? namespaceName;

        public string ClassFullName {
            get {
                if (!string.IsNullOrWhiteSpace(NamespaceName)) return $"{NamespaceName}.{ClassName}";
                else return ClassName;
            }
        }

        partial void OnClassNameChanged(string value) {
            if (string.IsNullOrWhiteSpace(value)) ClassName = DefaultClassName;
        }
    }
}
