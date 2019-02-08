using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EsapiEssentials.PluginRunner
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly PluginRunnerApp _app;

        public MainViewModel(PluginRunnerApp app)
        {
            _app = app;
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value);
        }

        private IEnumerable<PatientMatch> _patientMatches;
        public IEnumerable<PatientMatch> PatientMatches
        {
            get => _patientMatches;
            set => Set(ref _patientMatches, value);
        }

        public ICommand SearchPatientCommand => new RelayCommand(SearchPatient);

        private async void SearchPatient()
        {
            PatientMatches = await _app.FindPatientMatchesAsync(SearchText);
        }
    }
}