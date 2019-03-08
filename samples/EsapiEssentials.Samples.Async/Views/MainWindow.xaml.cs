using System.Windows;

namespace EsapiEssentials.Samples.Async
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Focus();
        }
    }
}
