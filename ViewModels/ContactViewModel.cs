using JobSearchTracker.Models;
using System;

namespace JobSearchTracker.ViewModels
{
    /// <summary>
    /// ViewModel for a Contact.
    /// </summary>
    public class ContactViewModel : ViewModelBase
    {
        private readonly Contact _contact;

        public ContactViewModel(Contact contact)
        {
            _contact = contact ?? throw new ArgumentNullException(nameof(contact));
        }

        public Contact Model => _contact;

        public Guid Id
        {
            get => _contact.Id;
            set => SetProperty(_contact.Id, value, () => _contact.Id = value);
        }

        public string Name
        {
            get => _contact.Name;
            set => SetProperty(_contact.Name, value, () => _contact.Name = value);
        }

        public string Email
        {
            get => _contact.Email;
            set => SetProperty(_contact.Email, value, () => _contact.Email = value);
        }

        public string Position
        {
            get => _contact.Position;
            set => SetProperty(_contact.Position, value, () => _contact.Position = value);
        }

        public string Notes
        {
            get => _contact.Notes;
            set => SetProperty(_contact.Notes, value, () => _contact.Notes = value);
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
