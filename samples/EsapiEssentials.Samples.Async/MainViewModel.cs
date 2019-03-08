using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EsapiEssentials.Samples.Async
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEsapiService _esapiService;
        private readonly IDialogService _dialogService;

        public MainViewModel(IEsapiService esapiService, IDialogService dialogService)
        {
            _esapiService = esapiService;
            _dialogService = dialogService;
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value);
        }

        private PatientMatch[] _patientMatches;
        public PatientMatch[] PatientMatches
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

        private Plan[] _plans;
        public Plan[] Plans
        {
            get => _plans;
            set => Set(ref _plans, value);
        }

        private Plan _selectedPlan;
        public Plan SelectedPlan
        {
            get => _selectedPlan;
            set => Set(ref _selectedPlan, value);
        }

        private ObservableCollection<MetricResult> _metricResults;
        public ObservableCollection<MetricResult> MetricResults
        {
            get => _metricResults;
            set => Set(ref _metricResults, value);
        }

        public ICommand StartCommand => new RelayCommand(Start);
        public ICommand SearchPatientCommand => new RelayCommand(SearchPatient);
        public ICommand OpenPatientCommand => new RelayCommand(OpenPatient);
        public ICommand AnalyzePlanCommand => new RelayCommand(AnalyzePlan);

        private async void Start()
        {
            _dialogService.ShowProgressDialog("Logging in to Eclipse. Please wait.");
            await _esapiService.LogInAsync();
            _dialogService.CloseProgressDialog();
        }

        private async void SearchPatient()
        {
            PatientMatches = await _esapiService.SearchAsync(SearchText);
        }

        private async void OpenPatient()
        {
            if (SelectedPatientMatch?.Id == null)
                return;

            await _esapiService.ClosePatientAsync();
            await _esapiService.OpenPatientAsync(SelectedPatientMatch.Id);
            Plans = await _esapiService.GetPlansAsync();
        }

        private async void AnalyzePlan()
        {
            var courseId = SelectedPlan?.CourseId;
            var planId = SelectedPlan?.PlanId;

            if (courseId == null || planId == null)
                return;

            var structureIds = await _esapiService.GetStructureIdsAsync(courseId, planId);

            _dialogService.ShowProgressDialog("Calculating dose metrics", structureIds.Length);

            MetricResults = new ObservableCollection<MetricResult>();
            foreach (var structureId in structureIds)
            {
                double result;

                try
                {
                    result = await _esapiService.CalculateMeanDoseAsync(courseId, planId, structureId);
                }
                catch
                {
                    result = double.NaN;
                }

                MetricResults.Add(new MetricResult
                {
                    StructureId = structureId,
                    Result = result
                });

                _dialogService.IncrementProgress();
            }

            _dialogService.CloseProgressDialog();
        }
    }
}