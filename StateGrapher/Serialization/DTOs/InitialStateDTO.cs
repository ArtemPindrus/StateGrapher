using System.Windows;

namespace StateGrapher.Serialization.DTOs {
    public record class InitialStateDTO(
        // base
        bool IsVisible, 
        Point Location, 
        Size Size, string? Name, Size DesiredSize) 
        : NodeDTO(IsVisible, Location, Size, Name, DesiredSize);
}
