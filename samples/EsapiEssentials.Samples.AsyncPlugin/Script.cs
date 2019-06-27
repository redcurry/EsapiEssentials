using EsapiEssentials.Plugin;
using EsapiEssentials.Samples.AsyncPlugin;

namespace VMS.TPS
{
    public class Script : ScriptBase
    {
        public override void Run(PluginScriptContext context)
        {
            using (var duplex = new Duplex())
            {
                duplex.Run(runner =>
                {
                    var window = new MainWindow();
                    var dialogService = new DialogService(window);
                    var esapiService = new EsapiService(runner, context);
                    var viewModel = new MainViewModel(esapiService, dialogService);
                    window.DataContext = viewModel;
                    window.ShowDialog();
                });
            }
        }
    }
}
