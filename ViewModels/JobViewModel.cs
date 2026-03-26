using JobSearchTracker.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace JobSearchTracker.ViewModels
{
    /// <summary>
    /// ViewModel for a Job.
    /// </summary>
    public class JobViewModel : ViewModelBase
    {
        private readonly Job _job;
        private bool _isSelected;

        public JobViewModel(Job job)
        {
            _job = job ?? throw new ArgumentNullException(nameof(job));
            
            Interviews = new ObservableCollection<InterviewViewModel>(
                _job.Interviews.Select(i => new InterviewViewModel(i))
            );

            Contacts = new ObservableCollection<ContactViewModel>(
                _job.Contacts.Select(c => new ContactViewModel(c))
            );
        }

        public Job Model => _job;

        public ObservableCollection<InterviewViewModel> Interviews { get; }
        public ObservableCollection<ContactViewModel> Contacts { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public Guid Id
        {
            get => _job.Id;
            set => SetProperty(_job.Id, value, () => _job.Id = value);
        }

        public string CompanyName
        {
            get => _job.CompanyName;
            set => SetProperty(_job.CompanyName, value, () => _job.CompanyName = value);
        }

        public string JobTitle
        {
            get => _job.JobTitle;
            set => SetProperty(_job.JobTitle, value, () => _job.JobTitle = value);
        }

        public string JobUrl
        {
            get => _job.JobUrl;
            set => SetProperty(_job.JobUrl, value, () => _job.JobUrl = value);
        }

        public string Location
        {
            get => _job.Location;
            set => SetProperty(_job.Location, value, () => _job.Location = value);
        }

        public string SalaryRange
        {
            get => _job.SalaryRange;
            set => SetProperty(_job.SalaryRange, value, () => _job.SalaryRange = value);
        }

        public DateTime? DatePosted
        {
            get => _job.DatePosted;
            set => SetProperty(_job.DatePosted, value, () => _job.DatePosted = value);
        }

        public DateTime? DateApplied
        {
            get => _job.DateApplied;
            set => SetProperty(_job.DateApplied, value, () => _job.DateApplied = value);
        }

        public ApplicationPlatform ApplicationPlatform
        {
            get => _job.ApplicationPlatform;
            set => SetProperty(_job.ApplicationPlatform, value, () => _job.ApplicationPlatform = value);
        }

        public JobStatus Status
        {
            get => _job.Status;
            set
            {
                if (SetProperty(_job.Status, value, () => _job.Status = value))
                {
                    OnPropertyChanged(nameof(IsRejected));
                }
            }
        }

        public string Description
        {
            get => _job.Description;
            set => SetProperty(_job.Description, value, () => _job.Description = value);
        }

        public string Notes
        {
            get => _job.Notes;
            set => SetProperty(_job.Notes, value, () => _job.Notes = value);
        }

        public DateTime? DateRejected
        {
            get => _job.DateRejected;
            set => SetProperty(_job.DateRejected, value, () => _job.DateRejected = value);
        }

        public DateTime? DateOffered
        {
            get => _job.DateOffered;
            set => SetProperty(_job.DateOffered, value, () => _job.DateOffered = value);
        }

        public DateTime DateAdded
        {
            get => _job.DateAdded;
            set => SetProperty(_job.DateAdded, value, () => _job.DateAdded = value);
        }

        public bool IsRejected => Status == JobStatus.Rejected;

        public string DisplayText => $"{CompanyName} - {JobTitle}";

        private bool SetProperty<T>(T oldValue, T newValue, Action updateAction, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            if (Equals(oldValue, newValue))
                return false;

            updateAction();
            OnPropertyChanged(propertyName);
            return true;
        }

        public void AddInterview(Interview interview)
        {
            _job.Interviews.Add(interview);
            Interviews.Add(new InterviewViewModel(interview));
        }

        public void RemoveInterview(InterviewViewModel interview)
        {
            _job.Interviews.Remove(interview.Model);
            Interviews.Remove(interview);
        }

        public void AddContact(Contact contact)
        {
            _job.Contacts.Add(contact);
            Contacts.Add(new ContactViewModel(contact));
        }

        public void RemoveContact(ContactViewModel contact)
        {
            _job.Contacts.Remove(contact.Model);
            Contacts.Remove(contact);
        }

        /// <summary>
        /// Refreshes all property bindings.
        /// </summary>
        public void RefreshAllProperties()
        {
            OnPropertyChanged(string.Empty);
        }
    }
}
