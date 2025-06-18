using System.Text.RegularExpressions;

namespace StateGrapher.Models {
    public record struct Transition(string Name, StateMachine From, StateMachine To, StateMachine Container) {
        public override string ToString() => $"{From.Name} > {Name} > {To.Name}";
    }
}
