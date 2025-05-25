using StateGrapher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StateGrapher.ViewModels
{
    public class StickyNodeViewModel : ViewModelBase {
        private readonly StickyNode node;

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
