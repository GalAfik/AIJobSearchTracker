using JobSearchTracker.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace JobSearchTracker.Converters
{
    /// <summary>
    /// Converts JobStatus.Rejected to a gray background color.
    /// </summary>
    public class JobStatusToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is JobStatus status && status == JobStatus.Rejected)
            {
                return new SolidColorBrush(Color.FromRgb(220, 220, 220));
            }

            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
