using System.Windows;

namespace EsapiEssentials.Samples.Async
{
    public class DialogService : IDialogService
    {
        private ProgressDialog _progressDialog;

        public void ShowProgressDialog(string message, int maximum)
        {
            Application.Current.MainWindow.IsEnabled = false;
            _progressDialog = new ProgressDialog();
            _progressDialog.Owner = Application.Current.MainWindow;
            _progressDialog.Message = message;
            _progressDialog.MaxProgress = maximum;
            _progressDialog.ResetProgress();
            _progressDialog.Show();
        }

        public void IncrementProgress()
        {
            _progressDialog.IncrementProgress();
        }

        public void CloseProgressDialog()
        {
            Application.Current.MainWindow.IsEnabled = true;
            _progressDialog.Close();
        }
    }
}