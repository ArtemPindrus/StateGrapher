using CommunityToolkit.Mvvm.ComponentModel;
using Nodify;
using System.Text.Json.Serialization;
using System.Windows.Controls;

namespace StateGrapher.Models
{
    public partial class Connection : Node, IEquatable<Connection> {
        [ObservableProperty]
        private ConnectionDirection direction;

        [ObservableProperty]
        private Orientation sourceOrientation;

        [ObservableProperty]
        private Orientation targetOrientation;

        [NotifyPropertyChangedFor(nameof(EventDisplayName))]
        [ObservableProperty]
        private bool isBothWays;

        /// <summary>
        /// Name of event that triggers transition from <see cref="From"/> to <see cref="To"/>.
        /// </summary>
        [NotifyPropertyChangedFor(nameof(EventDisplayName))]
        [ObservableProperty]
        private string? forwardEvent;

        /// <summary>
        /// Name of event that triggers transition from <see cref="To"/> to <see cref="From"/>.
        /// </summary>
        [NotifyPropertyChangedFor(nameof(EventDisplayName))]
        [ObservableProperty]
        private string? backEvent;

        public string? EventDisplayName {
            get {
                if (IsBothWays) {
                    if (ForwardEvent == BackEvent) return ForwardEvent;

                    return $"{ForwardEvent} / {BackEvent}";
                }

                return ForwardEvent;
            }
        }

        public Connector From { get; set; }
        public Connector To { get; set; }

        [Obsolete("For Json.")]
        [JsonConstructor]
        public Connection() {

        }

        public Connection(Connector from, Connector to) {
            From = from;
            To = to;
        }
        
        public bool Equals(Connection? other) => other != null 
            && From == other.From 
            && To == other.To;

        public override string? ToString() => $"{From.Container.Name} -> {To.Container.Name} using '{Name}' event";

        public override bool Equals(object? obj) {
            return Equals(obj as Connection);
        }

        public override int GetHashCode() {
            return HashCode.Combine(From, To);
        }

        protected override string? ValidateName(string? name) => name;
    }
}
