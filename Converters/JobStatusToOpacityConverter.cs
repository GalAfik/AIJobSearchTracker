using JobSearchTracker.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace JobSearchTracker.Converters
{
    /// <summary>
    /// Converts JobStatus.Rejected to a reduced opacity.
    /// </summary>
    public class JobStatusToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is JobStatus status && status == JobStatus.Rejected)
            {
                return 0.6;
            }

            return 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
