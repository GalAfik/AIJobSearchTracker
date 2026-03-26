using JobSearchTracker.Models;
using System;

namespace JobSearchTracker.ViewModels
{
    /// <summary>
    /// ViewModel for an Interview.
    /// </summary>
    public class InterviewViewModel : ViewModelBase
    {
        private readonly Interview _interview;

        public InterviewViewModel(Interview interview)
        {
            _interview = interview ?? throw new ArgumentNullException(nameof(interview));
        }

        public Interview Model => _interview;

        public Guid Id
        {
            get => _interview.Id;
            set => SetProperty(_interview.Id, value, () => _interview.Id = value);
        }

        public string InterviewerName
        {
            get => _interview.InterviewerName;
            set => SetProperty(_interview.InterviewerName, value, () => _interview.InterviewerName = value);
        }

        public string InterviewerEmail
        {
            get => _interview.InterviewerEmail;
            set => SetProperty(_interview.InterviewerEmail, value, () => _interview.InterviewerEmail = value);
        }

        public DateTime? InterviewDateTime
        {
            get => _interview.InterviewDateTime;
            set => SetProperty(_interview.InterviewDateTime, value, () => _interview.InterviewDateTime = value);
        }

        public string InterviewType
        {
            get => _interview.InterviewType;
            set => SetProperty(_interview.InterviewType, value, () => _interview.InterviewType = value);
        }

        public int Round
        {
            get => _interview.Round;
            set => SetProperty(_interview.Round, value, () => _interview.Round = value);
        }

        public string Notes
        {
            get => _interview.Notes;
            set => SetProperty(_interview.Notes, value, () => _interview.Notes = value);
        }

        public string Outcome
        {
            get => _interview.Outcome;
            set => SetProperty(_interview.Outcome, value, () => _interview.Outcome = value);
        }

        private bool SetProperty<T>(T oldValue, T newValue, Action updateAction, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            if (Equals(oldValue, newValue))
                return false;

            updateAction();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
