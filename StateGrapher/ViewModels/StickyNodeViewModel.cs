using StateGrapher.Models;
using System.Windows;

namespace StateGrapher.ViewModels
{
    public class StickyNodeViewModel : NodeViewModel {
        private readonly StickyNode node;

        public string? Text {
            get {
                return node.Text;
            }
            set {
                node.Text = value;
            }
        }

        public StickyNodeViewModel(StickyNode node) : base(node) {
            this.node = node;
        }
    }
}
