namespace StateGrapher.Models
{
    public class ExitNode : Node {
        public Connector Connector { get; set; }
        protected override string? ValidateName(string? name) => name?.Replace(" ", "");

        public ExitNode() {
            Connector = new(this);
        }

    }
}
