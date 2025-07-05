using Mapster;

namespace StateGrapher.Models {
    [AdaptTo("[name]DTO")]
    public record struct Graph(StateMachine RootStateMachine, Options Options);
}
