using System.Reflection;

namespace StateGrapher.Testing {
    public class TestingClassField {
        private readonly FieldInfo fieldInfo;
        private readonly object classInstance;

        public string Name => fieldInfo.Name;

        public bool Value {
            get => (bool)(fieldInfo.GetValue(classInstance) ?? false);
            set => fieldInfo.SetValue(classInstance, value);
        }

        public TestingClassField(FieldInfo fieldInfo, object classInstance) {
            this.fieldInfo = fieldInfo;
            this.classInstance = classInstance;
        }
    }
}
