using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace EsapiEssentials.PluginRunner
{
    public class PlansAndPlanSumsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<PlanOrPlanSum> planAndPlanSums)
                return string.Join(", ", planAndPlanSums.Select(p => p.Id));
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}