namespace EsapiEssentials.Samples.Async
{
    public interface IDialogService
    {
        void ShowProgressDialog(string message, int maximum = 0);
        void IncrementProgress();
        void CloseProgressDialog();
    }
}