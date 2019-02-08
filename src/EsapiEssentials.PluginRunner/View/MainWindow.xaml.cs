using System.Windows;

namespace EsapiEssentials.PluginRunner
{
    internal partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
