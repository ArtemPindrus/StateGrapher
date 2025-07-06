using System.Windows;

namespace StateGrapher.Serialization.DTOs {
    public record class ConnectionDTO(
        bool IsBothWays, 
        string? ForwardEvent, string? BackEvent, 
        int FromConnectorHashcode, int ToConnectorHashcode,
        
        // base
        bool IsVisible,
        string? Name) : NodeDTO(IsVisible, default, default, Name, default);
}
