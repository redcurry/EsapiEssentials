using System;
using System.Windows;
using System.Windows.Controls;

namespace EsapiEssentials.Samples.Async
{
    public partial class MainWindow : Window
    {
        private readonly EsapiService _esapiService;

        public MainWindow()
        {
            InitializeComponent();

            _esapiService = new EsapiService();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 0)
                MessageBox.Show(string.Join(" ", args));

            ProgressBar.IsIndeterminate = true;

            StatusTextBlock.Text = "Initializing ESAPI...";
            await _esapiService.LogInAsync("SysAdmin", "SysAdmin");

            StatusTextBlock.Text = "Opening patient...";
            await _esapiService.OpenPatientAsync("$DVHAnalysisQA");

            StatusTextBlock.Text = "Fetching courses...";
            var courseIds = await _esapiService.GetCourseIdsAsync();

            ListBox.ItemsSource = courseIds;

            StatusTextBlock.Text = "Calculating metric...";
            var metric = await _esapiService.CalculateMetricAsync(null, "HEADNECK", "HN IMRT ECL", "CORD");

            TextBlock.Text = metric.ToString("F2");

            StatusTextBlock.Text = "Done.";
            ProgressBar.IsIndeterminate = false;
        }

        private async void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var then = DateTime.Now;
            var results = await _esapiService.SearchAsync(SearchTextBox.Text);
            SearchResultsListBox.ItemsSource = results;
            var now = DateTime.Now;
            SearchTime.Text = $"{(now - then).Milliseconds} ms";
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            _esapiService.Dispose();
        }
    }
}
