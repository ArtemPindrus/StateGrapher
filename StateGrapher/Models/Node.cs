using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows;

namespace StateGrapher.Models {
    [JsonDerivedType(typeof(StateMachine), "StateMachine")]
    [JsonDerivedType(typeof(StickyNode), "Sticky")]
    [JsonDerivedType(typeof(InitialState), "InitialState")]
    [JsonDerivedType(typeof(ExitNode), "ExitNode")]
    public abstract partial class Node : ObservableObject {
        const int SnappingSize = 15;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private Point location;

        [ObservableProperty]
        private Size size;

        [ObservableProperty]
        private StateMachine? container;

        public Node() {
            name = ValidateName(name);
        }

        public virtual void ReactToConnectionAdded(ConnectionSource connectionSource, Connection connection) {

        }

        public virtual void ReactToConnectionRemoved() {

        }

        partial void OnNameChanged(string? value) {
            Name = ValidateName(value);
        }

        protected abstract string? ValidateName(string? name);

        partial void OnSizeChanged(Size value) {
            var correctedWidth = Math.Ceiling(value.Width / SnappingSize) * SnappingSize;
            var correctedHeight = Math.Ceiling(value.Height / SnappingSize) * SnappingSize;

            Size = new(correctedWidth, correctedHeight);
        }
    }

    public enum ConnectionSource {
        From,
        To,
    }
}
