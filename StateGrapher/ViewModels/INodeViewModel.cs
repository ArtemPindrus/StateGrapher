using StateGrapher.Models;
using System.Windows;

namespace StateGrapher.ViewModels
{
    public interface INodeViewModel {
        public Node Node { get; }

        public Point Location { get; set; }
    }

    public interface INodeViewModel<out T> : INodeViewModel where T : Node {
        public new T Node { get; }
    }
}
