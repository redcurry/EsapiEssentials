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

        // Must not be IEnumerable or it re-creates the view models
        // when accessed in OpenPatient(), losing which items are active
        private IList<PlanOrPlanSumViewModel> _plansAndPlanSums;
        public IList<PlanOrPlanSumViewModel> PlansAndPlanSums
        {
            get => _plansAndPlanSums;
            set => Set(ref _plansAndPlanSums, value);
        }

        public ICommand SearchPatientCommand => new RelayCommand(SearchPatient);
        public ICommand OpenPatientCommand => new RelayCommand(OpenPatient, CanOpenPatient);
        public ICommand RunCommand => new RelayCommand(Run);

        private void SearchPatient()
        {
            PatientMatches = _app.FindPatientMatchesAsync(SearchText);
        }

        private void OpenPatient()
        {
            var plansOrPlanSums = _app.GetPlansAndPlanSumsFor(SelectedPatientMatch.Id);
            PlansAndPlanSums = plansOrPlanSums.Select(CreatePlanOrPlanSumViewModel).ToList();
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

        private void Run()
        {
            var plansAndPlanSumsInScope = PlansAndPlanSums?.Select(CreatePlanOrPlanSum);
            var activePlanVm = PlansAndPlanSums?.FirstOrDefault(p => p.IsActive);
            var activePlan = activePlanVm != null ? CreatePlanOrPlanSum(activePlanVm) : null;
            _app.RunScript(SelectedPatientMatch?.Id, plansAndPlanSumsInScope, activePlan);
        }

        private PlanOrPlanSum CreatePlanOrPlanSum(PlanOrPlanSumViewModel vm) =>
            new PlanOrPlanSum
            {
                Type = vm.Type,
                Id = vm.Id,
                CourseId = vm.CourseId
            };
    }
}