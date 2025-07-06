using StateGrapher.Models;
using StateGrapher.ViewModels;
using System.ComponentModel;

namespace StateGrapher.Utilities
{
    public static class History {
        public static event PropertyChangedEventHandler? PropertyChanged;
        public static event EventHandler? LastSelectedNodeChanged;
        public static event EventHandler? LastSelectedConnectionChanged;
        public static event EventHandler? LastSelectedObjectChanged;

        private static string? lastActionHint;
        private static NodeViewModel? lastSelectedNode;
        private static ConnectionViewModel? lastSelectedConnection;

        public static string? LastActionHint {
            get => lastActionHint;
            set {
                lastActionHint = value;
                PropertyChanged?.Invoke(null, new(nameof(LastActionHint)));
            }
        }

        public static NodeViewModel? LastSelectedNode {
            get => lastSelectedNode;
            set {
                lastSelectedNode = value;

                LastSelectedNodeChanged?.Invoke(null, EventArgs.Empty);
                LastSelectedObjectChanged?.Invoke(null, EventArgs.Empty);

                if (value != null) LastSelectedObject = value;
            }
        }

        public static ConnectionViewModel? LastSelectedConnection {
            get => lastSelectedConnection;
            set {
                lastSelectedConnection = value;

                LastSelectedConnectionChanged?.Invoke(null, EventArgs.Empty);
                LastSelectedObjectChanged?.Invoke(null, EventArgs.Empty);

                if (value != null) LastSelectedObject = value;
            }
        }

        public static ViewModelBase? LastSelectedObject { get; private set; }
    }
}
