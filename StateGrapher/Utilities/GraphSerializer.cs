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

        public static bool SerializeToLast(StateMachine firstOrderStateMachine) {
            if (LastSerializationPath == null) return false;

            SerializeToFile(LastSerializationPath, firstOrderStateMachine);
            return true;
        }

        public static void SerializeToFile(string path, StateMachine firstOrderStateMachine) {
            Environment env = new Environment() { FirstOrderStateMachine = firstOrderStateMachine };
            string ser = JsonSerializer.Serialize<Environment>(env, jsonOptions);

            if (!string.IsNullOrEmpty(ser)) {
                File.WriteAllText(path, ser);

                History.LastActionHint = $"Serialized graph to file \"{path}\".";
                LastSerializationPath = path;
            } else {
                History.LastActionHint = $"Failed serialization to file \"{path}\".";
            }
        }

        public static bool DeserializeFromFile(string path, out Environment? environment) {
            if (!File.Exists(path) || Path.GetExtension(path) != ".json") {
                environment = default;
                return false;
            }

            string json = File.ReadAllText(path);
            environment = JsonSerializer.Deserialize<Environment>(json, jsonOptions);

            LastSerializationPath = path;
            History.LastActionHint = $"Deserialized graph \"{Path.GetFileNameWithoutExtension(path)}\"";

            return true;
        }
    }

    public class Environment {
        public StateMachine? FirstOrderStateMachine { get; set; }
    }
}
