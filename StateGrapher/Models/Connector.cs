using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows;

namespace StateGrapher.Models
{
    public partial class Connector : ObservableObject {
        [ObservableProperty]
        private Point anchor;

        [NotifyPropertyChangedFor(nameof(IsConnected))]
        [ObservableProperty]
        private int connections;

        public bool IsConnected => Connections > 0;

        public Node? Container { get; set; }

        [JsonConstructor]
        public Connector() {

        }

        public Connector(Node container) {
            Container = container;
        }
    }
}
