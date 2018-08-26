using System;
using System.Windows;
using EsapiEssentials.SampleServiceImpl;

namespace EsapiEssentials.WpfTest
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

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            _esapiService.Dispose();
        }
    }
}
