using Mapster;
using StateGrapher.Models;
using StateGrapher.Utilities;

namespace StateGrapher.Serialization.DTOs {
    public static class Mapper {
        static Mapper() {
            TypeAdapterConfig<Node, NodeDTO>
                .NewConfig()
                .MapToConstructor(true);

            TypeAdapterConfig<StateMachine, StateMachineDTO>.NewConfig();
            TypeAdapterConfig<InitialState, InitialStateDTO>.NewConfig();
            TypeAdapterConfig<Connection, ConnectionDTO>
                .NewConfig()
                .MapWith(c => new ConnectionDTO(c.IsBothWays,
                    c.ForwardEvent, c.BackEvent,
                    c.From.GetHashCode(), c.To.GetHashCode(),
                    c.IsVisible, c.Name));

            TypeAdapterConfig<Node, NodeDTO>
                .NewConfig()
                .Include<StateMachine, StateMachineDTO>()
                .Include<InitialState, InitialStateDTO>();
        }

        public static GraphDTO MapGraph(Graph graph) => graph.Adapt<GraphDTO>();

        public static StateMachine MapStateMachineBack(StateMachineDTO dto, StateMachine? container = null) {
            StateMachine sm = new(container) {
                IsExpanded = dto.isExpanded,

                IsVisible = dto.IsVisible,
                Location = dto.Location,
                Size = dto.Size,
                Name = dto.Name,
                DesiredSize = dto.DesiredSize,
            };

            foreach (var n in dto.Nodes) {
                if (n is StateMachineDTO nestedDto) sm.TryAddNode(MapStateMachineBack(nestedDto, sm));
                if (n is InitialStateDTO ini) sm.TryAddNode(MapInitialStateBack(ini, sm));
            }

            foreach (var c in dto.Connections) {
                var from = StateMachineUtility.FindChildConnector(sm, c.FromConnectorHashcode);
                var to = StateMachineUtility.FindChildConnector(sm, c.ToConnectorHashcode);

                if (from == null || to == null) continue;

                sm.TryAddConnection(from, to);
            }

            return sm;
        }

        public static InitialState MapInitialStateBack(InitialStateDTO dto, StateMachine container) {
            return new(container) {
                IsVisible = dto.IsVisible,
                Location = dto.Location,
                Size = dto.Size,
                Name = dto.Name,
                DesiredSize = dto.DesiredSize,
            };
        }

        public static Options MapOptionsBack(OptionsDTO dto) {
            Options options = new() {
                ClassName = dto.ClassName,
                NamespaceName = dto.NamespaceName
            };

            foreach (var b in dto.StateMachineBooleans) {
                options.StateMachineBooleans.Add(b.Adapt<StateMachineBool>());
            }

            return options;
        }
    }
}
