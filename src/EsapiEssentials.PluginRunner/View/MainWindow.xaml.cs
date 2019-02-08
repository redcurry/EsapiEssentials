using System.Windows;
using System.Windows.Controls;

namespace EsapiEssentials.PluginRunner
{
    internal partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void PlansAndPlanSums_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Prevent selection for this ListBox
            var listBox = (ListBox)sender;
            listBox.UnselectAll();
        }
    }
}
