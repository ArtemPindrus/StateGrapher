namespace StateGrapher.Models {
    public record struct Transition(string Name, StateMachine From, StateMachine To, StateMachine Container, IList<ConnectionCondition> Conditions) {
        public override string ToString() => $"{From.Name} > {Name} > {To.Name}";
    }
}
