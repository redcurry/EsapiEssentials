using System;
using System.Windows;
using EsapiPowerTools.SampleServiceImpl;

namespace EsapiPowerTools.WpfTest
{
    public partial class MainWindow : Window
    {
        private EsapiService _esapiService;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _esapiService = new EsapiService();
            await _esapiService.LogInAsync("SysAdmin", "SysAdmin");
            await _esapiService.OpenPatientAsync("$DVHAnalysisQA");
            var courseIds = await _esapiService.GetCourseIdsAsync();
            ListBox.ItemsSource = courseIds;
            var metric = await _esapiService.CalculateMetricAsync(null, "HEADNECK", "HN IMRT ECL", "CORD");
            TextBlock.Text = metric.ToString("F2");
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            _esapiService.Dispose();
        }
    }
}
