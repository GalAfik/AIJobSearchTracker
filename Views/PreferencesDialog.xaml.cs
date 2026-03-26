using JobSearchTracker.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JobSearchTracker.Views
{
    /// <summary>
    /// Interaction logic for PreferencesDialog.xaml
    /// </summary>
    public partial class PreferencesDialog : Window
    {
        private UserPreferences _preferences;

        public event EventHandler<AppTheme>? ThemeChanged;

        public PreferencesDialog(UserPreferences preferences)
        {
            InitializeComponent();
            _preferences = preferences ?? throw new ArgumentNullException(nameof(preferences));
            LoadPreferences();
        }

        public UserPreferences Preferences => _preferences;

        private void LoadPreferences()
        {
            // Set theme
            var themeItem = ThemeComboBox.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Tag?.ToString() == _preferences.Theme.ToString());
            
            if (themeItem != null)
            {
                ThemeComboBox.SelectedItem = themeItem;
            }

            // Set home address
            HomeAddressTextBox.Text = _preferences.HomeAddress;

            // Set sort option
            var sortItem = SortByComboBox.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content?.ToString() == _preferences.DefaultSortBy);
            
            if (sortItem != null)
            {
                SortByComboBox.SelectedItem = sortItem;
            }
            else
            {
                SortByComboBox.SelectedIndex = 0;
            }
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem is ComboBoxItem item && item.Tag != null)
            {
                var theme = Enum.Parse<AppTheme>(item.Tag.ToString()!);
                ThemeChanged?.Invoke(this, theme);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save theme
            if (ThemeComboBox.SelectedItem is ComboBoxItem themeItem && themeItem.Tag != null)
            {
                _preferences.Theme = Enum.Parse<AppTheme>(themeItem.Tag.ToString()!);
            }

            // Save home address
            _preferences.HomeAddress = HomeAddressTextBox.Text.Trim();

            // Save sort option
            if (SortByComboBox.SelectedItem is ComboBoxItem sortItem)
            {
                _preferences.DefaultSortBy = sortItem.Content?.ToString() ?? "Date Added (Newest)";
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
