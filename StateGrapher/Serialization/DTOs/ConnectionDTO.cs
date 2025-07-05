namespace StateGrapher.Serialization.DTOs {
    public record class ConnectionDTO(bool IsBothWays, string? ForwardEvent, string? BackEvent);
}
