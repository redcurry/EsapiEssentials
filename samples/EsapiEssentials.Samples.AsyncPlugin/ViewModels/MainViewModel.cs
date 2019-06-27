using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EsapiEssentials.Samples.AsyncPlugin
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

        private async void Start()
        {
            Plans = await _esapiService.GetPlansAsync();
        }

        public ICommand AnalyzePlanCommand => new RelayCommand(AnalyzePlan);

        private async void AnalyzePlan()
        {
            var courseId = SelectedPlan?.CourseId;
            var planId = SelectedPlan?.PlanId;

            if (courseId == null || planId == null)
                return;

            var structureIds = await _esapiService.GetStructureIdsAsync(courseId, planId);

            _dialogService.ShowProgressDialog("Calculating dose metrics", structureIds.Length,
                async progress =>
                {
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

                        progress.Increment();
                    }
                });
        }
    }
}