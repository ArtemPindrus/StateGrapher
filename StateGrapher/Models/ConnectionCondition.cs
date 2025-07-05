using System.Text.Json.Serialization;

namespace StateGrapher.Models {
    public class ConnectionCondition {
        public bool ShouldBeTrue { get; set; }
        public StateMachineBool SmBoolean { get; set; }

        [JsonConstructor]
        public ConnectionCondition() { }

        public ConnectionCondition(bool shouldBeTrue, StateMachineBool smBoolean) {
            ShouldBeTrue = shouldBeTrue;
            SmBoolean = smBoolean;
        }

        public override string ToString() => $"{SmBoolean.ToString()}: {ShouldBeTrue}";
    }
}
