using System.Windows;

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
    }
}
