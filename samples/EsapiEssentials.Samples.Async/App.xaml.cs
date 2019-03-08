using System.Windows;

namespace EsapiEssentials.Samples.Async
{
    public partial class App : Application
    {
        private EsapiService _esapiService;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _esapiService = new EsapiService();
            var dialogService = new DialogService();
            var viewModel = new MainViewModel(_esapiService, dialogService);
            var window = new MainWindow(viewModel);
            window.Show();
        }

        // The EsapiService must be disposed before exiting;
        // otherwise, an exception from ESAPI is thrown
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _esapiService.Dispose();
        }
    }
}
