using System.Windows;

namespace EsapiEssentials.Samples.Async
{
    public partial class ProgressDialog : Window
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        public string Message
        {
            get => MessageTextBlock.Text;
            set => MessageTextBlock.Text = value;
        }

        public int MaxProgress
        {
            get => (int)ProgressBar.Maximum;
            set
            {
                if (value <= 0)
                    ProgressBar.IsIndeterminate = true;
                else
                {
                    ProgressBar.Maximum = value;
                    ProgressBar.IsIndeterminate = false;
                }
            }
        }

        public void IncrementProgress()
        {
            ProgressBar.Value += 1;
        }

        public void ResetProgress()
        {
            ProgressBar.Value = 0;
        }
    }
}
