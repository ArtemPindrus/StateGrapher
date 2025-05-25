using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
