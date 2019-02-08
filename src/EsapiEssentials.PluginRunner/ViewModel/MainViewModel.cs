using System.Collections.Generic;
using System.Linq;
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

        private PatientMatch _selectedPatientMatch;
        public PatientMatch SelectedPatientMatch
        {
            get => _selectedPatientMatch;
            set => Set(ref _selectedPatientMatch, value);
        }

        private IEnumerable<PlanOrPlanSumViewModel> _plansAndPlanSums;
        public IEnumerable<PlanOrPlanSumViewModel> PlansAndPlanSums
        {
            get => _plansAndPlanSums;
            set => Set(ref _plansAndPlanSums, value);
        }

        public ICommand SearchPatientCommand => new RelayCommand(SearchPatient);
        public ICommand OpenPatientCommand => new RelayCommand(OpenPatient, CanOpenPatient);

        private async void SearchPatient()
        {
            PatientMatches = await _app.FindPatientMatchesAsync(SearchText);
        }

        private async void OpenPatient()
        {
            var plansOrPlanSums = await _app.GetPlansAndPlanSumsFor(SelectedPatientMatch.Id);
            PlansAndPlanSums = plansOrPlanSums.Select(CreatePlanOrPlanSumViewModel);
        }

        private bool CanOpenPatient() =>
            SelectedPatientMatch != null;

        private PlanOrPlanSumViewModel CreatePlanOrPlanSumViewModel(PlanOrPlanSum planOrPlanSum) =>
            new PlanOrPlanSumViewModel
            {
                Type = planOrPlanSum.Type,
                Id = planOrPlanSum.Id,
                CourseId = planOrPlanSum.CourseId
            };
    }
}