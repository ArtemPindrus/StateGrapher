using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows;

namespace StateGrapher.Models {
    [JsonDerivedType(typeof(StateMachine), "StateMachine")]
    [JsonDerivedType(typeof(StickyNode), "Sticky")]
    [JsonDerivedType(typeof(InitialState), "InitialState")]
    [JsonDerivedType(typeof(ExitNode), "ExitNode")]
    public abstract partial class Node : ObservableObject {
        /// <summary>
        /// Is node visible in NodeEditor?
        /// </summary>
        [ObservableProperty]
        private bool isVisible;

        /// <summary>
        /// Position of this node.
        /// </summary>
        [ObservableProperty]
        private Point location;

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private Size size;

        // TODO: should be of Node type?
        /// <summary>
        /// Node containing this node.
        /// </summary>
        public StateMachine? Container { get; set; }

        public Size DesiredSize { get; set; }

        public Node() {
            name = ValidateName(name);
        }

        public virtual void ReactToConnectionAdded(ConnectionSource connectionSource, Connection connection) {

        }

        public virtual void ReactToConnectionRemoved() {

        }

        partial void OnSizeChanged(Size value) {
            if (value != new Size(0,0)) DesiredSize = value;
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
