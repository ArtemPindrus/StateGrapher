using CommunityToolkit.Mvvm.Input;
using StateGrapher.Models;
using StateGrapher.Testing;

namespace StateGrapher.ViewModels {
    public class TestingViewModel : ViewModelBase {
        private readonly TestingEnvironment environment;

        public TestingClassField[] Fields { get; }

        public string[] EventIds => environment.EventIDs;

        public string CurrentState => environment.CurrentState;

        public TestingViewModel(TestingEnvironment environment) {
            this.environment = environment;

            Fields = new TestingClassField[environment.BooleanFields.Length];
            for (int i = 0; i < environment.BooleanFields.Length; i++) {
                var f = environment.BooleanFields[i];

                Fields[i] = new(f, environment.ClassInstance);
            }
        }

        public void DispatchTestingEvent(int eventId) {
            environment.DispatchEvent(eventId);
        }
    }
}
