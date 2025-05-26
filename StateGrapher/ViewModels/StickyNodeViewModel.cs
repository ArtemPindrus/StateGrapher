using StateGrapher.Models;
using System.Windows;

namespace StateGrapher.ViewModels
{
    public class StickyNodeViewModel : ViewModelBase, INodeViewModel<StickyNode> {
        private readonly StickyNode node;

        StickyNode INodeViewModel<StickyNode>.Node => node;

        Node INodeViewModel.Node => node;

        public string? Text {
            get {
                return node.Text;
            }
            set {
                node.Text = value;
            }
        }

        public Point Location {
            get => node.Location;
            set {
                node.Location = value;
            }
        }

        public StickyNodeViewModel(StickyNode node) {
            this.node = node;
        }
    }
}
