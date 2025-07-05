using CommunityToolkit.Mvvm.ComponentModel;

namespace StateGrapher.Models {
    /// <summary>
    /// Represents a public bool field within a state machine.
    /// </summary>
    public partial class StateMachineBool : ObservableObject {
        const string DefaultName = "UnnamedCondition";

        [ObservableProperty]
        private string name = DefaultName;

        [ObservableProperty]
        private bool initialState;

        partial void OnNameChanged(string value) {
            if (string.IsNullOrEmpty(value)) name = DefaultName;
        }

        public override string ToString() => Name;
    }
}
