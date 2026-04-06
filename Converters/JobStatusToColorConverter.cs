using JobSearchTracker.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace JobSearchTracker.Converters
{
    /// <summary>
    /// Converts JobStatus to an intuitive color for status badges.
    /// </summary>
    public class JobStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is JobStatus status)
            {
                return status switch
                {
                    JobStatus.NotApplied => new SolidColorBrush(Color.FromRgb(108, 117, 125)),    // Gray - neutral/inactive
                    JobStatus.Applied => new SolidColorBrush(Color.FromRgb(0, 123, 255)),         // Blue - in progress
                    JobStatus.Interviewed => new SolidColorBrush(Color.FromRgb(255, 193, 7)),     // Amber/Orange - important/active
                    JobStatus.Offered => new SolidColorBrush(Color.FromRgb(40, 167, 69)),         // Green - positive
                    JobStatus.Accepted => new SolidColorBrush(Color.FromRgb(25, 135, 84)),        // Dark Green - success
                    JobStatus.Rejected => new SolidColorBrush(Color.FromRgb(220, 53, 69)),        // Red - negative
                    JobStatus.Withdrawn => new SolidColorBrush(Color.FromRgb(108, 117, 125)),     // Gray - neutral/inactive
                    _ => new SolidColorBrush(Color.FromRgb(108, 117, 125))                        // Default gray
                };
            }

            return new SolidColorBrush(Color.FromRgb(108, 117, 125)); // Default gray
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
