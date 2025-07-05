namespace StateGrapher.Serialization.DTOs {
    public record struct OptionsDTO(string ClassName, string NamespaceName, StateMachineBoolDTO[] StateMachineBooleans);
}