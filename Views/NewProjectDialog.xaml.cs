using System.Windows;

namespace JobSearchTracker.Views
{
    /// <summary>
    /// Interaction logic for NewProjectDialog.xaml
    /// </summary>
    public partial class NewProjectDialog : Window
    {
        public NewProjectDialog()
        {
            InitializeComponent();
        }

        public string ProjectName { get; private set; } = string.Empty;
        public string ProjectDescription { get; private set; } = string.Empty;

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProjectNameTextBox.Text))
            {
                MessageBox.Show("Please enter a project name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProjectName = ProjectNameTextBox.Text.Trim();
            ProjectDescription = DescriptionTextBox.Text.Trim();
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
