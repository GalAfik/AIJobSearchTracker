using JobSearchTracker.Models;
using JobSearchTracker.ViewModels;
using System.Collections.Generic;
using System.Windows;

namespace JobSearchTracker.Views
{
    public partial class AnalyticsWindow : Window
    {
        public AnalyticsWindow(List<Job> jobs)
        {
            InitializeComponent();
            DataContext = new AnalyticsViewModel(jobs);
        }
    }
}
