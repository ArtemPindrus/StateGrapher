using StateGrapher.Models;
using StateGrapher.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StateGrapher.Utilities
{
    public static class History {
        public static event EventHandler<PropertyChangedEventArgs>? StaticPropertyChanged;

        private static string? lastActionHint;
        private static NodeViewModel? lastSelectedNode;
        private static ConnectionViewModel? lastSelectedConnection;
        private static ViewModelBase? lastSelectedObject;

        public static string? LastActionHint {
            get => lastActionHint;
            set {
                SetStaticProperty(ref lastActionHint, value);
            }
        }

        public static NodeViewModel? LastSelectedNode {
            get => lastSelectedNode;
            set {
                SetStaticProperty(ref lastSelectedNode, value);

                if (value != null) LastSelectedObject = value;
            }
        }

        public static ConnectionViewModel? LastSelectedConnection {
            get => lastSelectedConnection;
            set {
                SetStaticProperty(ref lastSelectedConnection, value);

                if (value != null) LastSelectedObject = value;
            }
        }

        public static ViewModelBase? LastSelectedObject { 
            get => lastSelectedObject; 
            private set {
                SetStaticProperty(ref lastSelectedObject, value);
            }
        }

        static void SetStaticProperty<T>(ref T property, T value, [CallerMemberName] string? propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(property, value)) return;

            property = value;

            StaticPropertyChanged?.Invoke(null, new(propertyName));
        }
    }
}
