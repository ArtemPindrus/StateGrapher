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

        public Connector From { get; set; }
        public Connector To { get; set; }

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

        public override bool Equals(object? obj) {
            return Equals(obj as Connection);
        }

        public override int GetHashCode() {
            return HashCode.Combine(From, To);
        }

        protected override string? ValidateName(string? name) => name;
    }
}
