using JobSearchTracker.Models;
using System;
using System.Windows;

namespace JobSearchTracker.Views
{
    /// <summary>
    /// Interaction logic for ContactDialog.xaml
    /// </summary>
    public partial class ContactDialog : Window
    {
        private readonly Contact _contact;

        public ContactDialog(Contact contact)
        {
            InitializeComponent();
            _contact = contact ?? throw new ArgumentNullException(nameof(contact));
            LoadData();
        }

        private void LoadData()
        {
            NameTextBox.Text = _contact.Name;
            EmailTextBox.Text = _contact.Email;
            PositionTextBox.Text = _contact.Position;
            NotesTextBox.Text = _contact.Notes;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || 
                string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Please enter name and email.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _contact.Name = NameTextBox.Text.Trim();
            _contact.Email = EmailTextBox.Text.Trim();
            _contact.Position = PositionTextBox.Text.Trim();
            _contact.Notes = NotesTextBox.Text.Trim();

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
