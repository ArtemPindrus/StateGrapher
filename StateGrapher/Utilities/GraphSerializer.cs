using StateGrapher.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StateGrapher.Utilities
{
    public static class GraphSerializer {
        private static readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true, ReferenceHandler = ReferenceHandler.Preserve };

        public static string? LastSerializationPath { get; private set; }

        public static bool SerializeToLast(Graph graph) {
            if (LastSerializationPath == null) return false;

            SerializeToFile(LastSerializationPath, graph);
            return true;
        }

        public static void SerializeToFile(string path, Graph graph) {
            string ser = JsonSerializer.Serialize<Graph>(graph, jsonOptions);

            if (!string.IsNullOrEmpty(ser)) {
                File.WriteAllText(path, ser);

                History.LastActionHint = $"Serialized graph to file \"{path}\".";
                LastSerializationPath = path;
            } else {
                History.LastActionHint = $"Failed serialization to file \"{path}\".";
            }
        }

        public static bool DeserializeFromFile(string path, out Graph? graph) {
            if (!File.Exists(path) || Path.GetExtension(path) != ".json") {
                graph = default;
                return false;
            }

            string json = File.ReadAllText(path);
            graph = JsonSerializer.Deserialize<Graph>(json, jsonOptions);

            LastSerializationPath = path;
            History.LastActionHint = $"Deserialized graph \"{Path.GetFileNameWithoutExtension(path)}\"";

            return true;
        }
    }
}
