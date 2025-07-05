using StateGrapher.Models;
using StateGrapher.Testing;
using StateGrapher.Utilities;
using StateGrapher.ViewModels;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace StateGrapher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Graph currentGraph;
        private static TestingEnvironment? testingEnvironment;


        public static event EventHandler<PropertyChangedEventArgs>? StaticPropertyChanged;

        public static ObservableCollection<StateMachineBool>? StateMachineBooleans => CurrentGraph.Options.StateMachineBooleans;

        public static Graph CurrentGraph {
            get => currentGraph; 
            private set => SetStaticProperty(ref currentGraph, value);
        }

        public static TestingEnvironment? TestingEnvironment { 
            get => testingEnvironment; 
            private set => SetStaticProperty(ref testingEnvironment, value);
        }

        protected override void OnStartup(StartupEventArgs e) {
            CreateNewGraph();

            MainViewModel mainViewModel = new MainViewModel(CurrentGraph);

            MainWindow mainWindow = new() {
                DataContext = mainViewModel
            };

            mainWindow.Show();

            base.OnStartup(e);
        }

        public static void CreateNewGraph() {
            StateMachine root = new() { Name = "ROOT", IsExpanded = true };

            Options op = new();

            CurrentGraph = new(root, op);
        }

        public static void RefreshTestingEnvironment() {
            string classString = StateMachineClassGenerator.GenerateCSharpClass(new(CurrentGraph.RootStateMachine, CurrentGraph.Options));

            TestingEnvironment = TestingEnvironment.FromGeneratedClass(classString, CurrentGraph.Options.ClassFullName);
        }

        public static void LoadGraph() {
            string? filePath = FolderDialog.RequestGraphPath();

            if (filePath is null) return;

            GraphSerializer.DeserializeFromFile(filePath, out Graph? graph);

            if (graph is not Graph validGraph
                || validGraph.RootStateMachine == null) return;

            CurrentGraph = validGraph;
        }

        protected static void SetStaticProperty<T>(ref T property, T value, [CallerMemberName] string? propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(property, value)) return;

            property = value;

            StaticPropertyChanged?.Invoke(null, new(propertyName));
        }
    }
}
