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
        private static NodeViewModel? lastSelectedConnection;

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
            }
        }

        public static NodeViewModel? LastSelectedConnection {
            get => lastSelectedConnection;
            set {
                lastSelectedConnection = value;

                LastSelectedConnectionChanged?.Invoke(null, EventArgs.Empty);
                LastSelectedObjectChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static NodeViewModel? LastSelectedObject => LastSelectedNode ?? LastSelectedConnection;
    }
}
