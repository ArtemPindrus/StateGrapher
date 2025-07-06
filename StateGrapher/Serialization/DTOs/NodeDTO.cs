using System.Text.Json.Serialization;
using System.Windows;

namespace StateGrapher.Serialization.DTOs {
    [JsonDerivedType(typeof(StateMachineDTO), "SmDto")]
    [JsonDerivedType(typeof(InitialStateDTO), "InitialStateDto")]
    public record class NodeDTO(bool IsVisible, Point Location, Size Size, string? Name, Size DesiredSize);
}
