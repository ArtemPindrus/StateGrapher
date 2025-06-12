using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace StateGrapher.Utilities
{
    public static class RegexUtil {
        private static readonly string TwoWayConnectionPattern = @"^\s*(\S+)\s*\/\s*(\S+)\s*$";

        public static TwoWayConnectionMatch MatchTwoWayConnection(string str) {
            var match = Regex.Match(str, TwoWayConnectionPattern);

            if (!match.Success) return new() { IsValid = false };

            string from_to = match.Groups[1].Value;
            string to_from = match.Groups[2].Value;

            bool valid = !string.IsNullOrWhiteSpace(from_to) 
                && !string.IsNullOrWhiteSpace(to_from);
            TwoWayConnectionMatch tw = new() {
                IsValid = valid,
                From_To_ToEventName = from_to,
                To_To_FromEventName = to_from
            };

            return tw;
        }
    }

    public readonly struct TwoWayConnectionMatch {
        public required bool IsValid { get; init; }
        public string From_To_ToEventName { get; init; }
        public string To_To_FromEventName { get; init; }
    }
}
