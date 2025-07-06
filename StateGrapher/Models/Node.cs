using CommunityToolkit.Mvvm.ComponentModel;
using Mapster;
using System.Text.Json.Serialization;
using System.Windows;

namespace StateGrapher.Models {
    [JsonDerivedType(typeof(StateMachine), "StateMachine")]
    [JsonDerivedType(typeof(StickyNode), "Sticky")]
    [JsonDerivedType(typeof(InitialState), "InitialState")]
    [JsonDerivedType(typeof(ExitNode), "ExitNode")]
    public abstract partial class Node : ObservableObject {
        /// <summary>
        /// Is node visible?
        /// </summary>
        [ObservableProperty]
        private bool isVisible;

        /// <summary>
        /// Node relative location within containing <see cref="StateMachine"/>.
        /// </summary>
        [ObservableProperty]
        private Point location;

        /// <summary>
        /// Node's size.
        /// </summary>
        [ObservableProperty]
        private Size size;

        [ObservableProperty]
        private string? name;

        // TODO: should be a Node?
        /// <summary>
        /// Container of this node.
        /// </summary>
        public StateMachine? Container { get; }

        public Size DesiredSize { get; set; }

        public Node(StateMachine? container) {
            name = ValidateName(name);

            Container = container;
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
