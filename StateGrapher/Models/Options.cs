using CommunityToolkit.Mvvm.ComponentModel;

namespace StateGrapher.Models
{
    public partial class Options : ObservableObject {
        [ObservableProperty]
        private string? className;

        [ObservableProperty]
        private string? namespaceName;
    }
}
