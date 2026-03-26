using JobSearchTracker.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace JobSearchTracker.Views
{
    /// <summary>
    /// Interaction logic for JobDialog.xaml
    /// </summary>
    public partial class JobDialog : Window
    {
        private readonly Job _job;
        private readonly ObservableCollection<Interview> _interviews;
        private readonly ObservableCollection<Contact> _contacts;

        public JobDialog(Job job)
        {
            InitializeComponent();
            _job = job ?? throw new ArgumentNullException(nameof(job));

            _interviews = new ObservableCollection<Interview>(_job.Interviews);
            _contacts = new ObservableCollection<Contact>(_job.Contacts);

            LoadData();
        }

        private void LoadData()
        {
            // Populate platform combo box
            PlatformComboBox.ItemsSource = Enum.GetValues(typeof(ApplicationPlatform));
            
            // Populate status combo box
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(JobStatus));

            // Set values
            CompanyNameTextBox.Text = _job.CompanyName;
            JobTitleTextBox.Text = _job.JobTitle;
            JobUrlTextBox.Text = _job.JobUrl;
            LocationTextBox.Text = _job.Location;
            SalaryRangeTextBox.Text = _job.SalaryRange;
            DatePostedPicker.SelectedDate = _job.DatePosted;
            DateAppliedPicker.SelectedDate = _job.DateApplied;
            PlatformComboBox.SelectedItem = _job.ApplicationPlatform;
            StatusComboBox.SelectedItem = _job.Status;
            DescriptionTextBox.Text = _job.Description;
            NotesTextBox.Text = _job.Notes;

            InterviewsDataGrid.ItemsSource = _interviews;
            ContactsDataGrid.ItemsSource = _contacts;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CompanyNameTextBox.Text) || 
                string.IsNullOrWhiteSpace(JobTitleTextBox.Text))
            {
                MessageBox.Show("Please enter company name and job title.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _job.CompanyName = CompanyNameTextBox.Text.Trim();
            _job.JobTitle = JobTitleTextBox.Text.Trim();
            _job.JobUrl = JobUrlTextBox.Text.Trim();
            _job.Location = LocationTextBox.Text.Trim();
            _job.SalaryRange = SalaryRangeTextBox.Text.Trim();
            _job.DatePosted = DatePostedPicker.SelectedDate;
            _job.DateApplied = DateAppliedPicker.SelectedDate;
            _job.ApplicationPlatform = (ApplicationPlatform)PlatformComboBox.SelectedItem;
            _job.Status = (JobStatus)StatusComboBox.SelectedItem;
            _job.Description = DescriptionTextBox.Text.Trim();
            _job.Notes = NotesTextBox.Text.Trim();

            if (_job.Status == JobStatus.Rejected && !_job.DateRejected.HasValue)
            {
                _job.DateRejected = DateTime.Now;
            }

            if (_job.Status == JobStatus.Offered && !_job.DateOffered.HasValue)
            {
                _job.DateOffered = DateTime.Now;
            }

            _job.Interviews.Clear();
            foreach (var interview in _interviews)
            {
                _job.Interviews.Add(interview);
            }

            _job.Contacts.Clear();
            foreach (var contact in _contacts)
            {
                _job.Contacts.Add(contact);
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void AddInterviewButton_Click(object sender, RoutedEventArgs e)
        {
            var interview = new Interview { Round = _interviews.Count + 1 };
            var dialog = new InterviewDialog(interview);
            
            if (dialog.ShowDialog() == true)
            {
                _interviews.Add(interview);
            }
        }

        private void EditInterviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (InterviewsDataGrid.SelectedItem is Interview interview)
            {
                var dialog = new InterviewDialog(interview);
                dialog.ShowDialog();
            }
        }

        private void RemoveInterviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (InterviewsDataGrid.SelectedItem is Interview interview)
            {
                var result = MessageBox.Show("Are you sure you want to remove this interview?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    _interviews.Remove(interview);
                }
            }
        }

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            var contact = new Contact();
            var dialog = new ContactDialog(contact);
            
            if (dialog.ShowDialog() == true)
            {
                _contacts.Add(contact);
            }
        }

        private void EditContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContactsDataGrid.SelectedItem is Contact contact)
            {
                var dialog = new ContactDialog(contact);
                dialog.ShowDialog();
            }
        }

        private void RemoveContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContactsDataGrid.SelectedItem is Contact contact)
            {
                var result = MessageBox.Show("Are you sure you want to remove this contact?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    _contacts.Remove(contact);
                }
            }
        }

        private void EmailContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.DataContext is Contact contact)
            {
                ComposeEmail(contact.Email, $"Re: {_job.JobTitle} at {_job.CompanyName}");
            }
        }

        private void GmailContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.DataContext is Contact contact)
            {
                ComposeGmail(contact.Email, $"Re: {_job.JobTitle} at {_job.CompanyName}");
            }
        }

        private void OutlookContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.DataContext is Contact contact)
            {
                ComposeOutlook(contact.Email, $"Re: {_job.JobTitle} at {_job.CompanyName}");
            }
        }

        private void ComposeEmail(string email, string subject)
        {
            try
            {
                var emailService = new Services.EmailService();
                emailService.ComposeEmail(email, subject);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open email client: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComposeGmail(string email, string subject)
        {
            try
            {
                var emailService = new Services.EmailService();
                emailService.ComposeGmail(email, subject);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open Gmail: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComposeOutlook(string email, string subject)
        {
            try
            {
                var emailService = new Services.EmailService();
                emailService.ComposeOutlookWeb(email, subject);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open Outlook: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
