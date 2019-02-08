using GalaSoft.MvvmLight;

namespace EsapiEssentials.PluginRunner
{
    internal class PlanOrPlanSumViewModel : ViewModelBase
    {
        private PlanType _type;
        public PlanType Type
        {
            get => _type;
            set
            {
                if (Set(ref _type, value))
                    RaisePropertyChanged(nameof(CanBeActive));
            }
        }

        private string _id;
        public string Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        private string _courseId;
        public string CourseId
        {
            get => _courseId;
            set => Set(ref _courseId, value);
        }

        private bool _isInScope;
        public bool IsInScope
        {
            get => _isInScope;
            set
            {
                if (Set(ref _isInScope, value))
                {
                    if (!IsInScope)
                        IsActive = false;
                    RaisePropertyChanged(nameof(CanBeActive));
                }
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => Set(ref _isActive, value);
        }

        public bool CanBeActive =>
            Type == PlanType.Plan && IsInScope;
    }
}