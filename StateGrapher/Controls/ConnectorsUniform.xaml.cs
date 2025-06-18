using System.Windows.Controls;

namespace StateGrapher.Controls {
    /// <summary>
    /// Interaction logic for ConnectorsUniform.xaml
    /// </summary>
    public partial class ConnectorsUniform : UserControl {
        public int Columns { get; set; }
        public int Rows { get; set; }

        public ConnectorsUniform() {
            InitializeComponent();
        }
    }
}
