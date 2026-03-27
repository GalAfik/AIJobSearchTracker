using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace JobSearchTracker.Views
{
    public partial class WelcomeDialog : Window
    {
        public WelcomeDialog()
        {
            InitializeComponent();
        }

        public bool DontShowAgain => DontShowAgainCheckBox.IsChecked ?? false;

        private void GetStartedButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void SponsorLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
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
                // Silently ignore if browser fails to open
            }
        }
    }
}
