using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace StateGrapher.Models
{
    public partial class Connection : ObservableObject, IEquatable<Connection> {
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
    }
}
