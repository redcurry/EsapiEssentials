using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using EsapiEssentials.Standalone;
using Application = VMS.TPS.Common.Model.API.Application;

namespace EsapiEssentials.Samples.StandaloneRunner.App
{
    public partial class MainWindow : Window
    {
        private Application _app;
        private StandaloneScriptContext _scriptContext;

        public MainWindow()
        {
            InitializeComponent();
            InitializeEsapi();
            InitializeControls();
        }

        private void InitializeEsapi()
        {
            // The ESAPI Application object must be created before the PluginScriptContext
            _app = Application.CreateApplication(null, null);

            // The PluginScriptContext requires the command-line arguments and the Application object
            // (if there are no command-line arguments, the return is null)
            _scriptContext = StandaloneScriptContext.From(Environment.GetCommandLineArgs(), _app);
        }

        private void InitializeControls()
        {
            // Display some information from the created PluginScriptContext object (if not null)
            // (for simplicity, we're not using data-binding but directly accessing the controls)
            PatientTextBlock.Text = $"Patient: {_scriptContext?.Patient?.LastName}, {_scriptContext?.Patient?.FirstName}";
            CourseTextBlock.Text = $"Course: {_scriptContext?.Course?.Id}";
            PlanTextBlock.Text = $"Plan: {_scriptContext?.PlanSetup?.Id}";
            PlanSumsInScopeTextBlock.Text = $"Plan sums in scope: {GetFormattedPlanSumIds()}";
        }

        private string GetFormattedPlanSumIds() =>
            string.Join(", ", _scriptContext?.PlanSumsInScope?.Select(x => x.Id) ?? new string[0]);

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Always dispose of the ESAPI Application object before exiting the program
            _app.Dispose();
        }
    }
}
