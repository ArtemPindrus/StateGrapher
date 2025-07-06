namespace StateGrapher.Models
{
    public class ExitNode : Node {
        public Connector Connector { get; set; }
        protected override string? ValidateName(string? name) => name?.Replace(" ", "");

        public ExitNode(StateMachine container) : base(container) {
            Connector = new(this);
        }

    }
}
