using System.Text;

namespace StateGrapher.Models {
    public record struct Transition(string Name, StateMachine From, StateMachine To, StateMachine Container, IList<ConnectionCondition> Conditions) {
        public readonly string GetConditionsString() {
            if (Conditions.Count == 0) return "";

            StringBuilder s = new("if (");

            bool firstIteration = true;
            foreach (var condition in Conditions) {
                if (!firstIteration) s.Append(" && ");

                if (!condition.ShouldBeTrue) s.Append('!');
                s.Append(condition.SmBoolean.Name);

                firstIteration = false;
            }

            s.Append(')');

            return s.ToString();
        }
        
        public readonly override string ToString() => $"{From.Name} > {GetConditionsString()} > through {Name} > {To.Name}";
    }
}
