using JobSearchTracker.Models;
using System;
using System.Windows;

namespace JobSearchTracker.Views
{
    /// <summary>
    /// Interaction logic for InterviewDialog.xaml
    /// </summary>
    public partial class InterviewDialog : Window
    {
        private readonly Interview _interview;

        public InterviewDialog(Interview interview)
        {
            InitializeComponent();
            _interview = interview ?? throw new ArgumentNullException(nameof(interview));
            LoadData();
        }

        private void LoadData()
        {
            RoundTextBox.Text = _interview.Round.ToString();
            InterviewTypeTextBox.Text = _interview.InterviewType;
            InterviewDatePicker.SelectedDate = _interview.InterviewDateTime?.Date;
            
            if (_interview.InterviewDateTime.HasValue)
            {
                InterviewTimeTextBox.Text = _interview.InterviewDateTime.Value.ToString("HH:mm");
            }

            InterviewerNameTextBox.Text = _interview.InterviewerName;
            InterviewerEmailTextBox.Text = _interview.InterviewerEmail;
            OutcomeTextBox.Text = _interview.Outcome;
            NotesTextBox.Text = _interview.Notes;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(RoundTextBox.Text, out int round) || round < 1)
            {
                MessageBox.Show("Please enter a valid round number (1 or greater).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _interview.Round = round;
            _interview.InterviewType = InterviewTypeTextBox.Text.Trim();
            _interview.InterviewerName = InterviewerNameTextBox.Text.Trim();
            _interview.InterviewerEmail = InterviewerEmailTextBox.Text.Trim();
            _interview.Outcome = OutcomeTextBox.Text.Trim();
            _interview.Notes = NotesTextBox.Text.Trim();

            // Combine date and time
            if (InterviewDatePicker.SelectedDate.HasValue)
            {
                var date = InterviewDatePicker.SelectedDate.Value;
                
                if (TimeSpan.TryParse(InterviewTimeTextBox.Text, out TimeSpan time))
                {
                    _interview.InterviewDateTime = date.Date + time;
                }
                else
                {
                    _interview.InterviewDateTime = date;
                }
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
