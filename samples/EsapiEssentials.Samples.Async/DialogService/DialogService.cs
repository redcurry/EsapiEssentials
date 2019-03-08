using System;
using System.Threading.Tasks;

namespace EsapiEssentials.Samples.Async
{
    public class DialogService : IDialogService
    {
        private readonly App _app;

        public DialogService(App app)
        {
            _app = app;
        }

        public void ShowProgressDialog(string message, Func<ISimpleProgress, Task> workAsync)
        {
            CreateProgressDialog(message, workAsync).ShowDialog();
        }

        public void ShowProgressDialog(string message, int maximum, Func<ISimpleProgress, Task> workAsync)
        {
            CreateProgressDialog(message, maximum, workAsync).ShowDialog();
        }

        private ProgressDialog CreateProgressDialog(string message, Func<ISimpleProgress, Task> workAsync)
        {
            var progressDialog = new ProgressDialog();
            progressDialog.Owner = _app.MainWindow;
            progressDialog.Message = message;
            progressDialog.Loaded += async (sender, args) =>
            {
                var progress = new SimpleProgress(() => progressDialog.IncrementProgress());
                await workAsync(progress);
                progressDialog.Close();
            };
            return progressDialog;
        }

        private ProgressDialog CreateProgressDialog(string message, int maximum, Func<ISimpleProgress, Task> workAsync)
        {
            var progressDialog = CreateProgressDialog(message, workAsync);
            progressDialog.MaxProgress = maximum;
            return progressDialog;
        }
    }
}