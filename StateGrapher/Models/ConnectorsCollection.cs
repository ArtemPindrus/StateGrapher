using System.Text.Json.Serialization;

namespace StateGrapher.Models
{
    public class ConnectorsCollection {
        public Connector[] LeftConnectors { get; set; }
        public Connector[] TopConnectors { get; set; }
        public Connector[] RightConnectors { get; set; }
        public Connector[] BottomConnectors { get; set; }

        [JsonConstructor]
        [Obsolete("This constructor is for JSON serializer.", true)]
        public ConnectorsCollection() { }

        public ConnectorsCollection(Node container, int left, int top, int right, int bottom) { 
            LeftConnectors = new Connector[left];
            TopConnectors = new Connector[top];
            RightConnectors = new Connector[right];
            BottomConnectors = new Connector[bottom];

            Init(LeftConnectors, container);
            Init(TopConnectors, container);
            Init(RightConnectors, container);
            Init(BottomConnectors, container);
        }

        public ConnectorsCollection(Node container, int symmetricLength) {
            LeftConnectors = new Connector[symmetricLength];
            TopConnectors = new Connector[symmetricLength];
            RightConnectors = new Connector[symmetricLength];
            BottomConnectors = new Connector[symmetricLength];

            Init(LeftConnectors, container);
            Init(TopConnectors, container);
            Init(RightConnectors, container);
            Init(BottomConnectors, container);
        }

        private static void Init(Connector[] collection, Node container) {
            for (int i = 0; i < collection.Length; i++) {
                collection[i] = new(container);
            }
        }
    }
}
