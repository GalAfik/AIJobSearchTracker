using JobSearchTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobSearchTracker.ViewModels
{
    public class AnalyticsViewModel : ViewModelBase
    {
        private readonly List<Job> _jobs;

        public AnalyticsViewModel(List<Job> jobs)
        {
            _jobs = jobs ?? new List<Job>();
            CalculateAnalytics();
        }

        // Overall Statistics
        private int _totalApplications;
        public int TotalApplications
        {
            get => _totalApplications;
            set => SetProperty(ref _totalApplications, value);
        }

        private int _activeApplications;
        public int ActiveApplications
        {
            get => _activeApplications;
            set => SetProperty(ref _activeApplications, value);
        }

        private int _totalInterviews;
        public int TotalInterviews
        {
            get => _totalInterviews;
            set => SetProperty(ref _totalInterviews, value);
        }

        private int _totalOffers;
        public int TotalOffers
        {
            get => _totalOffers;
            set => SetProperty(ref _totalOffers, value);
        }

        private int _totalRejections;
        public int TotalRejections
        {
            get => _totalRejections;
            set => SetProperty(ref _totalRejections, value);
        }

        // Response Rates
        private double _interviewRate;
        public double InterviewRate
        {
            get => _interviewRate;
            set => SetProperty(ref _interviewRate, value);
        }

        private double _offerRate;
        public double OfferRate
        {
            get => _offerRate;
            set => SetProperty(ref _offerRate, value);
        }

        private double _rejectionRate;
        public double RejectionRate
        {
            get => _rejectionRate;
            set => SetProperty(ref _rejectionRate, value);
        }

        private double _interviewToOfferRate;
        public double InterviewToOfferRate
        {
            get => _interviewToOfferRate;
            set => SetProperty(ref _interviewToOfferRate, value);
        }

        // Status Breakdown
        private int _notAppliedCount;
        public int NotAppliedCount
        {
            get => _notAppliedCount;
            set => SetProperty(ref _notAppliedCount, value);
        }

        private int _appliedCount;
        public int AppliedCount
        {
            get => _appliedCount;
            set => SetProperty(ref _appliedCount, value);
        }

        private int _interviewedCount;
        public int InterviewedCount
        {
            get => _interviewedCount;
            set => SetProperty(ref _interviewedCount, value);
        }

        private int _offeredCount;
        public int OfferedCount
        {
            get => _offeredCount;
            set => SetProperty(ref _offeredCount, value);
        }

        private int _acceptedCount;
        public int AcceptedCount
        {
            get => _acceptedCount;
            set => SetProperty(ref _acceptedCount, value);
        }

        private int _rejectedCount;
        public int RejectedCount
        {
            get => _rejectedCount;
            set => SetProperty(ref _rejectedCount, value);
        }

        private int _withdrawnCount;
        public int WithdrawnCount
        {
            get => _withdrawnCount;
            set => SetProperty(ref _withdrawnCount, value);
        }

        // Time-based metrics
        private double _averageDaysToResponse;
        public double AverageDaysToResponse
        {
            get => _averageDaysToResponse;
            set => SetProperty(ref _averageDaysToResponse, value);
        }

        private int _applicationsThisWeek;
        public int ApplicationsThisWeek
        {
            get => _applicationsThisWeek;
            set => SetProperty(ref _applicationsThisWeek, value);
        }

        private int _applicationsThisMonth;
        public int ApplicationsThisMonth
        {
            get => _applicationsThisMonth;
            set => SetProperty(ref _applicationsThisMonth, value);
        }

        private int _currentStreak;
        public int CurrentStreak
        {
            get => _currentStreak;
            set => SetProperty(ref _currentStreak, value);
        }

        // Platform statistics
        private string _mostSuccessfulPlatform;
        public string MostSuccessfulPlatform
        {
            get => _mostSuccessfulPlatform;
            set => SetProperty(ref _mostSuccessfulPlatform, value);
        }

        private string _mostUsedPlatform;
        public string MostUsedPlatform
        {
            get => _mostUsedPlatform;
            set => SetProperty(ref _mostUsedPlatform, value);
        }

        // Additional insights
        private string _busiestDay;
        public string BusiestDay
        {
            get => _busiestDay;
            set => SetProperty(ref _busiestDay, value);
        }

        private string _busiestMonth;
        public string BusiestMonth
        {
            get => _busiestMonth;
            set => SetProperty(ref _busiestMonth, value);
        }

        private int _upcomingInterviews;
        public int UpcomingInterviews
        {
            get => _upcomingInterviews;
            set => SetProperty(ref _upcomingInterviews, value);
        }

        private void CalculateAnalytics()
        {
            if (_jobs == null || !_jobs.Any())
            {
                SetDefaultValues();
                return;
            }

            // Overall statistics
            TotalApplications = _jobs.Count;
            ActiveApplications = _jobs.Count(j => j.Status != JobStatus.Rejected && 
                                                  j.Status != JobStatus.Withdrawn && 
                                                  j.Status != JobStatus.Accepted);
            TotalInterviews = _jobs.Count(j => j.Interviews?.Any() == true);
            TotalOffers = _jobs.Count(j => j.Status == JobStatus.Offered || j.Status == JobStatus.Accepted);
            TotalRejections = _jobs.Count(j => j.Status == JobStatus.Rejected);

            // Status breakdown
            NotAppliedCount = _jobs.Count(j => j.Status == JobStatus.NotApplied);
            AppliedCount = _jobs.Count(j => j.Status == JobStatus.Applied);
            InterviewedCount = _jobs.Count(j => j.Status == JobStatus.Interviewed);
            OfferedCount = _jobs.Count(j => j.Status == JobStatus.Offered);
            AcceptedCount = _jobs.Count(j => j.Status == JobStatus.Accepted);
            RejectedCount = _jobs.Count(j => j.Status == JobStatus.Rejected);
            WithdrawnCount = _jobs.Count(j => j.Status == JobStatus.Withdrawn);

            // Response rates
            var appliedJobs = _jobs.Where(j => j.DateApplied.HasValue).ToList();
            if (appliedJobs.Any())
            {
                InterviewRate = (double)TotalInterviews / appliedJobs.Count * 100;
                OfferRate = (double)TotalOffers / appliedJobs.Count * 100;
                RejectionRate = (double)TotalRejections / appliedJobs.Count * 100;
            }

            if (TotalInterviews > 0)
            {
                InterviewToOfferRate = (double)TotalOffers / TotalInterviews * 100;
            }

            // Time-based metrics
            CalculateTimeMetrics();
            CalculatePlatformStats();
            CalculateAdditionalInsights();
        }

        private void CalculateTimeMetrics()
        {
            var now = DateTime.Now;

            // Applications this week
            var startOfWeek = now.AddDays(-(int)now.DayOfWeek);
            ApplicationsThisWeek = _jobs.Count(j => j.DateApplied.HasValue && 
                                                     j.DateApplied.Value >= startOfWeek);

            // Applications this month
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            ApplicationsThisMonth = _jobs.Count(j => j.DateApplied.HasValue && 
                                                      j.DateApplied.Value >= startOfMonth);

            // Average days to response
            var jobsWithResponse = _jobs.Where(j => j.DateApplied.HasValue && 
                                                     j.Interviews?.Any() == true).ToList();
            if (jobsWithResponse.Any())
            {
                var responseTimes = jobsWithResponse.Select(j =>
                {
                    var firstInterview = j.Interviews.OrderBy(i => i.InterviewDateTime).FirstOrDefault();
                    if (firstInterview != null && 
                        firstInterview.InterviewDateTime.HasValue && 
                        j.DateApplied.HasValue)
                    {
                        return (firstInterview.InterviewDateTime.Value - j.DateApplied.Value).TotalDays;
                    }
                    return 0;
                }).Where(d => d > 0);

                if (responseTimes.Any())
                {
                    AverageDaysToResponse = responseTimes.Average();
                }
            }

            // Current streak (consecutive days with applications)
            CalculateStreak();

            // Upcoming interviews
            UpcomingInterviews = _jobs.SelectMany(j => j.Interviews ?? new List<Interview>())
                                      .Count(i => i.InterviewDateTime.HasValue && i.InterviewDateTime.Value > DateTime.Now);
        }

        private void CalculateStreak()
        {
            var sortedDates = _jobs.Where(j => j.DateApplied.HasValue)
                                   .Select(j => j.DateApplied.Value.Date)
                                   .Distinct()
                                   .OrderByDescending(d => d)
                                   .ToList();

            if (!sortedDates.Any())
            {
                CurrentStreak = 0;
                return;
            }

            var streak = 0;
            var currentDate = DateTime.Now.Date;

            // Check if there's activity today or yesterday
            if (sortedDates.First() != currentDate && sortedDates.First() != currentDate.AddDays(-1))
            {
                CurrentStreak = 0;
                return;
            }

            var expectedDate = sortedDates.First();
            foreach (var date in sortedDates)
            {
                if (date == expectedDate || date == expectedDate.AddDays(-1))
                {
                    streak++;
                    expectedDate = date.AddDays(-1);
                }
                else
                {
                    break;
                }
            }

            CurrentStreak = streak;
        }

        private void CalculatePlatformStats()
        {
            var platformGroups = _jobs.GroupBy(j => j.ApplicationPlatform)
                                      .Select(g => new
                                      {
                                          Platform = g.Key,
                                          Count = g.Count(),
                                          OfferRate = g.Count(j => j.Status == JobStatus.Offered || 
                                                                    j.Status == JobStatus.Accepted)
                                      })
                                      .OrderByDescending(p => p.Count)
                                      .ToList();

            if (platformGroups.Any())
            {
                MostUsedPlatform = platformGroups.First().Platform.ToString();
                
                var mostSuccessful = platformGroups.OrderByDescending(p => p.OfferRate).FirstOrDefault();
                if (mostSuccessful != null && mostSuccessful.OfferRate > 0)
                {
                    MostSuccessfulPlatform = mostSuccessful.Platform.ToString();
                }
                else
                {
                    MostSuccessfulPlatform = "N/A";
                }
            }
        }

        private void CalculateAdditionalInsights()
        {
            // Busiest day of week
            var dayGroups = _jobs.Where(j => j.DateApplied.HasValue)
                                 .GroupBy(j => j.DateApplied.Value.DayOfWeek)
                                 .OrderByDescending(g => g.Count())
                                 .FirstOrDefault();
            BusiestDay = dayGroups?.Key.ToString() ?? "N/A";

            // Busiest month
            var monthGroups = _jobs.Where(j => j.DateApplied.HasValue)
                                   .GroupBy(j => j.DateApplied.Value.ToString("MMMM yyyy"))
                                   .OrderByDescending(g => g.Count())
                                   .FirstOrDefault();
            BusiestMonth = monthGroups?.Key ?? "N/A";
        }

        private void SetDefaultValues()
        {
            TotalApplications = 0;
            ActiveApplications = 0;
            TotalInterviews = 0;
            TotalOffers = 0;
            TotalRejections = 0;
            InterviewRate = 0;
            OfferRate = 0;
            RejectionRate = 0;
            InterviewToOfferRate = 0;
            NotAppliedCount = 0;
            AppliedCount = 0;
            InterviewedCount = 0;
            OfferedCount = 0;
            AcceptedCount = 0;
            RejectedCount = 0;
            WithdrawnCount = 0;
            AverageDaysToResponse = 0;
            ApplicationsThisWeek = 0;
            ApplicationsThisMonth = 0;
            CurrentStreak = 0;
            MostSuccessfulPlatform = "N/A";
            MostUsedPlatform = "N/A";
            BusiestDay = "N/A";
            BusiestMonth = "N/A";
            UpcomingInterviews = 0;
        }
    }
}
