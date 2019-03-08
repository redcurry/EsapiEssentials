namespace EsapiEssentials.Samples.Async
{
    public class DialogService : IDialogService
    {
        private readonly LogInWaitDialog _logInWaitDialog;

        public DialogService()
        {
            _logInWaitDialog = new LogInWaitDialog();
        }

        public void ShowLogInWaitDialog()
        {
            _logInWaitDialog.Show();
        }

        public void CloseLogInWaitDialog()
        {
            _logInWaitDialog.Close();
        }
    }
}