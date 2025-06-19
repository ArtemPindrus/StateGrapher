using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace StateGrapher.Models
{
    public class TestingEnvironment {
        private readonly object classInstance;
        private readonly FieldInfo stateIdField;
        private readonly MethodInfo dispatchEventMethod;

        public string CurrentState { get; private set; }

        public string[] EventIDs { get; private set; }
        public string[] StateIDs { get; private set; }

        public TestingEnvironment(object classInstance, FieldInfo stateIdField, MethodInfo dispatchEventMethod, string[] eventIDs, string[] stateIDs, string currentState) {
            this.classInstance = classInstance;
            this.stateIdField = stateIdField;
            this.dispatchEventMethod = dispatchEventMethod;
            EventIDs = eventIDs;
            CurrentState = currentState;
            StateIDs = stateIDs;
        }

        public void DispatchEvent(int eventId) {
            dispatchEventMethod.Invoke(classInstance, [eventId]);

            CurrentState = StateIDs[(int)stateIdField.GetValue(classInstance)];
        }

        public static TestingEnvironment? FromGeneratedClass(string classString, string className) {
            var syntaxTree = CSharpSyntaxTree.ParseText(classString);
            var assemblyName = "DynamicAssembly_" + Guid.NewGuid();
            var compilation = CSharpCompilation.Create(assemblyName,
                [syntaxTree],
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(CompilationReference.CreateFromFile(typeof(object).Assembly.Location));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success) return null;

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);

            // Create instance
            var classType = assembly.GetType(className);

            if (classType == null) return null;

            var classInstance = Activator.CreateInstance(classType);

            if (classInstance == null) return null;

            var startMethod = classType.GetMethod("Start",
                System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance);

            if (startMethod == null) return null;

            startMethod.Invoke(classInstance, null);

            var eventIdType = classType.GetNestedType("EventId");
            var stateIdType = classType.GetNestedType("StateId");

            if (eventIdType == null
                || stateIdType == null) return null;

            string[] eventIdMembers = eventIdType.GetEnumNames();
            string[] stateIdMembers = stateIdType.GetEnumNames();

            var stateIdField = classType.GetField("stateId",
                System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance);

            var dispatchEventMethod = classType.GetMethod("DispatchEvent",
                System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance);

            if (dispatchEventMethod == null) return null;

            var currentState = stateIdMembers[(int)stateIdField.GetValue(classInstance)];
            var testingEnv = new TestingEnvironment(classInstance, stateIdField, dispatchEventMethod, eventIdMembers, stateIdMembers, currentState);

            return testingEnv;    
        }
    }
}
