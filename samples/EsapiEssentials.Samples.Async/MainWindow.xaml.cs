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

        /*
        private async void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var then = DateTime.Now;
            var results = await _esapiService.SearchAsync(SearchTextBox.Text);
            SearchResultsListBox.ItemsSource = results;
            var now = DateTime.Now;
            SearchTime.Text = $"{(now - then).Milliseconds} ms";
        }
        */
    }
}
