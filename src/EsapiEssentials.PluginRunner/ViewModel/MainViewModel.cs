using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EsapiEssentials.PluginRunner
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly PluginRunner _runner;

        public MainViewModel(PluginRunner runner)
        {
            _runner = runner;
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
        public ICommand OpenPatientCommand => new RelayCommand(OpenPatient);
        public ICommand RunCommand => new RelayCommand(Run);

        private void SearchPatient()
        {
            PatientMatches = _runner.FindPatientMatches(SearchText);

            SelectedPatientMatch = null;
            PlansAndPlanSums = null;
        }

        private void OpenPatient()
        {
            var plansAndPlanSums = _runner.GetPlansAndPlanSumsFor(SelectedPatientMatch?.Id);
            PlansAndPlanSums = plansAndPlanSums?.Select(CreatePlanOrPlanSumViewModel).ToList();
        }

        private PlanOrPlanSumViewModel CreatePlanOrPlanSumViewModel(PlanOrPlanSum planOrPlanSum) =>
            new PlanOrPlanSumViewModel
            {
                Type = planOrPlanSum.Type,
                Id = planOrPlanSum.Id,
                CourseId = planOrPlanSum.CourseId
            };

        private void Run()
        {
            _runner.RunScript(SelectedPatientMatch?.Id, GetPlansAndPlanSumsInScope(), GetActivePlan());
        }

        private PlanOrPlanSum[] GetPlansAndPlanSumsInScope() =>
            PlansAndPlanSums?.Where(p => p.IsInScope).Select(CreatePlanOrPlanSum).ToArray();

        private PlanOrPlanSum GetActivePlan() =>
            PlansAndPlanSums?.Where(p => p.IsActive).Select(CreatePlanOrPlanSum).FirstOrDefault();

        private PlanOrPlanSum CreatePlanOrPlanSum(PlanOrPlanSumViewModel vm) =>
            new PlanOrPlanSum
            {
                Type = vm.Type,
                Id = vm.Id,
                CourseId = vm.CourseId
            };
    }
}