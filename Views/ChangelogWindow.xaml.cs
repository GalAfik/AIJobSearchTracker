using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace JobSearchTracker.Views
{
    /// <summary>
    /// Interaction logic for ChangelogWindow.xaml
    /// Displays the changelog and release notes for the application.
    /// </summary>
    public partial class ChangelogWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the ChangelogWindow.
        /// </summary>
        public ChangelogWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Close button click event.
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Handles hyperlink navigation requests.
        /// </summary>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
                e.Handled = true;
            }
            catch
            {
                MessageBox.Show("Failed to open URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
