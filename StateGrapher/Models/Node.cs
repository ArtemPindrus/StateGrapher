using CommunityToolkit.Mvvm.ComponentModel;
using System.Numerics;
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

        public Size DesiredSize { get; set; }

        public Node() {
            name = ValidateName(name);
        }

        public virtual void ReactToConnectionAdded(ConnectionSource connectionSource, Connection connection) {

        }

        public virtual void ReactToConnectionRemoved() {

        }

        partial void OnSizeChanged(Size value) {
            if (value != new Size(0,0)) {
                double correctedWidth = ((int)value.Width / SnappingSize) * SnappingSize;
                double correctedHeight = ((int)value.Height / SnappingSize) * SnappingSize;

                DesiredSize = new(correctedWidth, correctedHeight);
            }
        }

        partial void OnNameChanged(string? value) {
            Name = ValidateName(value);
        }

        protected abstract string? ValidateName(string? name);
    }

    public enum ConnectionSource {
        From,
        To,
    }
}
