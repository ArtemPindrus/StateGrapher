using Mapster;
using StateGrapher.Models;
using StateGrapher.Serialization.DTOs;
using StateGrapher.Utilities;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StateGrapher.Serialization
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
            GraphDTO graphDto = graph.Adapt<GraphDTO>();

            string ser = JsonSerializer.Serialize(graphDto, jsonOptions);

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
            var graphDto = JsonSerializer.Deserialize<GraphDTO>(json, jsonOptions);

            var options = MapOptions(graphDto.Options);

            graph = new(null, options);

            LastSerializationPath = path;
            History.LastActionHint = $"Deserialized graph \"{Path.GetFileNameWithoutExtension(path)}\"";

            return true;
        }

        public static Options MapOptions(OptionsDTO dto) {
            Options options = new() {
                ClassName = dto.ClassName,
                NamespaceName = dto.NamespaceName
            };

            foreach (var b in dto.StateMachineBooleans) {
                options.StateMachineBooleans.Add(b.Adapt<StateMachineBool>());
            }

            return options;
        }
    }
}
