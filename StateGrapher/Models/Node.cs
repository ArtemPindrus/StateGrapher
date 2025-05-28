using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows;

namespace StateGrapher.Models {
    [JsonDerivedType(typeof(StateMachine), "StateMachine")]
    [JsonDerivedType(typeof(StickyNode), "Sticky")]
    [JsonDerivedType(typeof(InitialState), "InitialState")]
    [JsonDerivedType(typeof(ExitNode), "ExitNode")]
    public abstract partial class Node : ObservableObject {
        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private Point location;

        [ObservableProperty]
        private Size size;

        public Size DesiredSize { get; set; }

        public Node() {
            name = ValidateName(name);
        }

        partial void OnSizeChanged(Size value) {
            if (value != new Size(0,0)) DesiredSize = value;
        }

        partial void OnNameChanged(string? value) {
            Name = ValidateName(value);
        }

        protected abstract string? ValidateName(string? name);
    }
}
