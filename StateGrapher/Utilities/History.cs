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
        private static INodeViewModel? lastSelectedNode;
        private static INodeViewModel? lastSelectedConnection;

        public static string? LastActionHint {
            get => lastActionHint;
            set {
                lastActionHint = value;
                PropertyChanged?.Invoke(null, new(nameof(LastActionHint)));
            }
        }

        public static INodeViewModel? LastSelectedNode {
            get => lastSelectedNode;
            set {
                lastSelectedNode = value;

                LastSelectedNodeChanged?.Invoke(null, EventArgs.Empty);
                LastSelectedObjectChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static INodeViewModel? LastSelectedConnection {
            get => lastSelectedConnection;
            set {
                lastSelectedConnection = value;

                LastSelectedConnectionChanged?.Invoke(null, EventArgs.Empty);
                LastSelectedObjectChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static INodeViewModel? LastSelectedObject => LastSelectedNode ?? LastSelectedConnection;
    }
}
