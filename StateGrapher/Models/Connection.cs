using CommunityToolkit.Mvvm.ComponentModel;
using Nodify;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Controls;

namespace StateGrapher.Models
{
    public partial class Connection : ObservableObject, IEquatable<Connection> {
        private ObservableCollection<ConnectionCondition> forwardConditions = new();
        private ObservableCollection<ConnectionCondition> backwardsConditions = new();

        [ObservableProperty]
        private string? name;

        [NotifyPropertyChangedFor(nameof(EventDisplayName))]
        [ObservableProperty]
        private bool isBothWays;

        /// <summary>
        /// Whether to display conditions with events string in NodeEditor.
        /// </summary>
        [NotifyPropertyChangedFor(nameof(EventDisplayName))]
        [ObservableProperty]
        private bool displayConditions;

        /// <summary>
        /// Name of event that triggers transition from <see cref="From"/> to <see cref="To"/>.
        /// </summary>
        [NotifyPropertyChangedFor(nameof(EventDisplayName))]
        [ObservableProperty]
        private string? forwardEvent;

        /// <summary>
        /// Name of event that triggers transition from <see cref="To"/> to <see cref="From"/>.
        /// </summary>
        [NotifyPropertyChangedFor(nameof(EventDisplayName))]
        [ObservableProperty]
        private string? backEvent;

        public string? EventDisplayName {
            get {
                if (IsBothWays) {
                    StringBuilder s = new();

                    if (DisplayConditions) s.Append(GetConditionsString(ForwardConditions));

                    s.Append($"{ForwardEvent} / ");

                    if (DisplayConditions) s.Append(GetConditionsString(BackwardsConditions));

                    s.Append(BackEvent);

                    return s.ToString();
                }

                StringBuilder st = new();

                if (DisplayConditions) st.Append(GetConditionsString(ForwardConditions));

                st.Append(ForwardEvent);

                return st.ToString();
            }
        }

        public ObservableCollection<ConnectionCondition> ForwardConditions {
            get => forwardConditions;
            set {
                forwardConditions = value;
                forwardConditions.CollectionChanged += ConditionsCollectionChanged;
            }
        }

        public ObservableCollection<ConnectionCondition> BackwardsConditions {
            get => backwardsConditions;
            set {
                backwardsConditions = value;
                backwardsConditions.CollectionChanged += ConditionsCollectionChanged;
            }
        }

        public StateMachine? Container { get; set; }

        public Connector? From { get; set; }
        public Connector? To { get; set; }

        [JsonConstructor]
        public Connection() {
        }

        public Connection(Connector from, Connector to) {
            From = from;
            To = to;
        }

        private void ConditionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged(nameof(EventDisplayName));
        }

        public void RemoveCondition(StateMachineBool boolean) {
            if (ForwardConditions.FirstOrDefault(x => x.SmBoolean == boolean) is ConnectionCondition cc) {
                ForwardConditions.Remove(cc);
            }

            if (BackwardsConditions.FirstOrDefault(x => x.SmBoolean == boolean) is ConnectionCondition cc1) {
                BackwardsConditions.Remove(cc1);
            }
        }

        public bool Equals(Connection? other) => other != null 
            && From == other.From 
            && To == other.To;

        public static string GetConditionsString(IEnumerable<ConnectionCondition> conditions) {
            if (!conditions.Any()) return "";

            StringBuilder s = new("if (");

            bool firstIteration = true;
            foreach (var condition in conditions) {
                if (!firstIteration) s.Append(" && ");

                if (!condition.ShouldBeTrue) s.Append('!');

                s.Append(condition.SmBoolean.Name);

                firstIteration = false;
            }

            s.Append(')');

            return s.ToString();
        }

        public override string? ToString() => $"{From?.Container.Name} -> {To?.Container.Name} using '{Name}' event";

        public override bool Equals(object? obj) {
            return Equals(obj as Connection);
        }

        public override int GetHashCode() {
            return HashCode.Combine(From, To);
        }
    }
}
