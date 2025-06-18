using StateGrapher.Extensions;
using StateGrapher.Models;
using StateGrapher.Utilities;

namespace StateGrapher.Tests {
    public class StateMachineTests {
        [Fact]
        public void GetLeastCommonAncestor_ReturnsValidNode() {
            StateMachine root = new StateMachine() { Name = "Root"}.TryAddNodes(
                new StateMachine() { Name = "Standing" },
                new StateMachine() { Name = "RunningLock" }.TryAddNodes(
                    new StateMachine() { Name = "IsUncrouching" }
                )
            );

            root.TryAddConnection("Standing", "RunningLock").ForwardEvent = "CTRL";
            var connection = root.TryAddConnection("RunningLock", "Standing");
            connection.ForwardEvent = "CTRL";

            Assert.NotNull(connection);

            List<Transition> transitions = StateMachineUtility.ToTransitions([connection]);

            Assert.True(transitions.Count > 0);

            var lca = StateMachineUtility.GetLeastCommonAncestor(transitions[0]);

            Assert.True(lca.Name == "Root");
        }
    }
}
