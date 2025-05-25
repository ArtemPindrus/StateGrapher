using Microsoft.Win32;
using System.IO;

namespace StateGrapher.Utilities
{
    public static class FolderDialog {
        public static string? RequestSaveGraphPath() {
            var dialog = new OpenFileDialog {
                Title = "Select a folder",
                CheckFileExists = false,
                CheckPathExists = false,
                Filter = "Folder Selection.|*.*",
                InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyComputer)
            };

            if (dialog.ShowDialog() == true) {
                var path = dialog.FileName;
                if (Path.GetExtension(path) != ".json") path += ".json";

                return path;
            }

            return null;
        }

        public static string? RequestGraphPath() {
            var dialog = new OpenFileDialog {
                Title = "Select a graph file.",
                CheckFileExists = false,
                CheckPathExists = false,
                FileName = "Graph selection.",
                Filter = "JSON files (*.json)|*.json",
                InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyComputer)
            };

            if (dialog.ShowDialog() == true) {
                return dialog.FileName;
            }

            return null;
        }
    }
}
