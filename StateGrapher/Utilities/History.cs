using System.ComponentModel;

namespace StateGrapher.Utilities
{
    public static class History {
        public static event PropertyChangedEventHandler? PropertyChanged;

        private static string? lastActionHint;

        public static string? LastActionHint {
            get => lastActionHint;
            set {
                lastActionHint = value;
                PropertyChanged?.Invoke(null, new(nameof(LastActionHint)));
            }
        }
    }
}
