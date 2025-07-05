namespace StateGrapher.Serialization.DTOs {
    public record struct GraphDTO(StateMachineDTO RootStateMachine, OptionsDTO Options);
}
