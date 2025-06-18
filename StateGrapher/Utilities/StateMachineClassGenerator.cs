using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Infrastructure;
using StateGrapher.Extensions;
using StateGrapher.Models;
using System.Buffers;
using System.Security.Permissions;

namespace StateGrapher.Utilities
{
    public static class StateMachineClassGenerator
    {
        /// <summary>
        /// Generates C# state-machine class based on <see cref="StateMachine"/> instance.
        /// </summary>
        /// <param name="rootSm"></param>
        public static string GenerateCSharpClass(StateMachine rootSm) {
            string name = rootSm.Name ?? "StateMachine";

            // open class
            IndentedStringBuilder classString = new IndentedStringBuilder()
                .AppendLines($$"""
                using System;

                public partial class {{name}} {
                """)
                .IncrementIndent();

            var allStates = rootSm.GetHierarchyNodes(true).ToArray();
            var allConnections = rootSm.GetHierarchyConnections().ToArray();
            var allTransitions = StateMachineUtility.ToTransitions(allConnections);

            AppendEventIdEnum(classString, allTransitions).AppendLine();
            AppendStateIdEnum(classString, allStates).AppendLine();
            
            classString.AppendLine("public StateId stateId;")
                .AppendLine();

            AppendConstructor(classString, rootSm);
            AppendStartMethod(classString, rootSm);
            AppendDispatchEventsMethod(classString, allStates, allTransitions);
            AppendStringToEventIdMethod(classString, allTransitions);

            foreach (var s in allStates) {
                AppendEventHandlers(classString, s, allTransitions);
            }

            // close class
            classString.DecrementIndent()
                .AppendLine("}");

            var str = classString.ToString();
            return str;
        }

        private static IndentedStringBuilder AppendStringToEventIdMethod(IndentedStringBuilder sb,
            IEnumerable<Transition> allTransitions) {
            sb.AppendBlock("public EventId StringToEventId(string s) => s switch", (sb) => {
                foreach (var t in allTransitions.DistinctBy(x => x.Name)) {
                    sb.AppendLine($"\"{t.Name}\" => EventId.{t.Name},");
                }

                sb.AppendLine($"\"Update\" => EventId.Update,");

                sb.AppendLine($"_ => throw new ArgumentException(\"Failed to find EventId.\"),");
            }, ";").AppendLine();

            return sb;
        }

        private static IndentedStringBuilder AppendDispatchEventsMethod(IndentedStringBuilder sb,
            IEnumerable<StateMachine> allStates, IEnumerable<Transition> allTransitions) {
            sb.AppendBlock("public void DispatchEvent(EventId eventId)", (sb) => {
                sb.AppendBlock("switch (stateId)", (sb) => {
                    foreach (var state in allStates) {
                        var stateTransitions = allTransitions.Where(x => x.From == state);

                        sb.AppendLines($"""
                            // FOR STATE: {state.Name}
                            case StateId.{state.Name}:
                            """)
                            .IncrementIndent();

                        if (state.Name != "ROOT"
                            && stateTransitions.Any()) {
                            sb.AppendBlock("switch (eventId)", (sb) => {
                                foreach (var t in stateTransitions) {
                                    sb.AppendLine($"case EventId.{t.Name}: {state.Name}_{t.Name}(); break;");
                                }

                                sb.AppendLine($"case EventId.Update: {state.Name}_Update(); break;");
                            });
                        }

                        sb.AppendLine("break;")
                            .DecrementIndent()
                            .AppendLine();
                    }
                });
            }).AppendLine();

            return sb;
        }

        private static IndentedStringBuilder AppendStartMethod(IndentedStringBuilder sb, StateMachine rootSm) {
            if (rootSm.InitialState == null) return sb;

            sb.AppendLines($$"""
                public void Start() {
                    ROOT_Enter();
                    {{rootSm.InitialState.Connection.To.Container.Name}}_Enter();
                }
                 
                """);

            return sb;
        }

        private static IndentedStringBuilder AppendConstructor(IndentedStringBuilder sb, StateMachine sm) {
            sb.AppendLines($$"""
                public {{sm.Name}}(){
                }
                 
                """);

            return sb;
        }

        private static IndentedStringBuilder AppendEventHandlers(IndentedStringBuilder sb, 
            StateMachine sm, 
            IEnumerable<Transition> transitions) {
            // start region
            sb.AppendLines($"""
                #region Event handlers for {sm.Name}

                """);

            if (sm.Name == "ROOT") {
                sb.AppendLines("""
                    private void ROOT_Enter() {
                        stateId = StateId.ROOT;
                    }

                    #endregion
                     
                    """);

                return sb;
            }

            // enter handler
            sb.AppendLines($$"""
                private void {{sm.Name}}_Enter() {
                    stateId = StateId.{{sm.Name}};
                    On{{sm.Name}}_Enter();
                }
                partial void On{{sm.Name}}_Enter();
                 
                """);

            // update handler
            sb.AppendLines($$"""
                private void {{sm.Name}}_Update() {
                    On{{sm.Name}}_Update();
                }
                partial void On{{sm.Name}}_Update();
                 
                """);

            // exit handler
            sb.AppendLines($$"""
                private void {{sm.Name}}_Exit() {
                    stateId = StateId.{{sm.Container.Name}};
                    On{{sm.Name}}_Exit();
                }
                partial void On{{sm.Name}}_Exit();
                 
                """);

            foreach (var transition in transitions.Where(x => x.From == sm)) {
                sb.AppendBlock($"private void {sm.Name}_{transition.Name}()", (sb) => {
                    sb.AppendLines($"""
                        // exit to the Least Common Ancestor
                        {sm.Name}_Exit();
                        """);

                    // exit to the LCA
                    var lca = StateMachineUtility.GetLeastCommonAncestor(transition);
                    var container = sm.Container;
                    while (container != lca) {
                        sb.AppendLine($"{container.Name}_Exit();");
                        container = container.Container;
                    }

                    sb.AppendLine();

                    // perform transition action
                    sb.AppendLines($"""
                    // perform transition action
                    On{sm.Name}_{transition.Name}();

                    """);

                    // enter other state
                    var current = transition.To;

                    string enterTree = "";
                    while (current != lca && current != null) {
                        enterTree = enterTree.Insert(0, $"{current.Name}_Enter();\n");
                        current = current.Container;
                    }

                    sb.AppendLines($"""
                    // enter other state
                    {enterTree}
                    """);
                }).AppendLine($"partial void On{sm.Name}_{transition.Name}();");
            }

            // close region
            sb.AppendLine("#endregion").AppendLine();

            return sb;
        }

        private static IndentedStringBuilder AppendEventIdEnum(IndentedStringBuilder sb, IEnumerable<Transition> transitions) {
            sb.AppendBlock("public enum EventId", (sb) => {
                foreach (var t in transitions.DistinctBy(x => x.Name)) {
                    sb.AppendLine($"{t.Name},");
                }

                sb.AppendLine("Update,");
            });

            return sb;
        }

        private static IndentedStringBuilder AppendStateIdEnum(IndentedStringBuilder sb, IEnumerable<StateMachine> states) {
            sb.AppendEnum("public", "StateId", states.Select(x => x.Name));

            return sb;
        }
    }
}
