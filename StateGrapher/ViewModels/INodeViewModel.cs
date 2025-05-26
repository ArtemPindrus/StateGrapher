using StateGrapher.Models;

namespace StateGrapher.ViewModels
{
    public interface INodeViewModel {
        public Node Node { get; }
    }

    public interface INodeViewModel<out T> : INodeViewModel where T : Node {
        public new T Node { get; }
    }
}
