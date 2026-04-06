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
        private bool _isLoading = true;

        public event EventHandler<AppTheme>? ThemeChanged;

        public PreferencesDialog(UserPreferences preferences)
        {
            InitializeComponent();
            _preferences = preferences ?? throw new ArgumentNullException(nameof(preferences));
            LoadPreferences();
            _isLoading = false;
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

            // Set compact view
            CompactViewCheckBox.IsChecked = _preferences.UseCompactView;

            // Set show intro on startup
            ShowIntroCheckBox.IsChecked = _preferences.ShowIntroOnStartup;

            // Set check for updates
            CheckForUpdatesCheckBox.IsChecked = _preferences.CheckForUpdates;

            // Set suppress warnings
            SuppressWarningsCheckBox.IsChecked = _preferences.SuppressWarnings;

            // Set auto-save after import
            AutoSaveCheckBox.IsChecked = _preferences.AutoSaveAfterImport;

            // Set home address fields
            StreetTextBox.Text = _preferences.Street;
            CityTextBox.Text = _preferences.City;
            StateTextBox.Text = _preferences.State;
            ZipCodeTextBox.Text = _preferences.ZipCode;
            CountryTextBox.Text = _preferences.Country;

            // Set up address preview
            StreetTextBox.TextChanged += UpdateAddressPreview;
            CityTextBox.TextChanged += UpdateAddressPreview;
            StateTextBox.TextChanged += UpdateAddressPreview;
            ZipCodeTextBox.TextChanged += UpdateAddressPreview;
            CountryTextBox.TextChanged += UpdateAddressPreview;
            UpdateAddressPreview(null, null);

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

            // Set API keys (if they exist, show placeholder)
            if (!string.IsNullOrWhiteSpace(_preferences.ClaudeApiKey))
            {
                ClaudeApiKeyBox.Password = _preferences.ClaudeApiKey;
            }

            if (!string.IsNullOrWhiteSpace(_preferences.OpenAiApiKey))
            {
                OpenAiApiKeyBox.Password = _preferences.OpenAiApiKey;
            }

            if (!string.IsNullOrWhiteSpace(_preferences.GeminiApiKey))
            {
                GeminiApiKeyBox.Password = _preferences.GeminiApiKey;
            }

            // Set preferred AI provider
            var aiProviderItem = PreferredAiProviderComboBox.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Tag?.ToString() == _preferences.PreferredAiProvider);

            if (aiProviderItem != null)
            {
                PreferredAiProviderComboBox.SelectedItem = aiProviderItem;
            }
        }

        private void UpdateAddressPreview(object? sender, System.Windows.Controls.TextChangedEventArgs? e)
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(StreetTextBox?.Text))
                parts.Add(StreetTextBox.Text);

            if (!string.IsNullOrWhiteSpace(CityTextBox?.Text))
                parts.Add(CityTextBox.Text);

            if (!string.IsNullOrWhiteSpace(StateTextBox?.Text))
                parts.Add(StateTextBox.Text);

            if (!string.IsNullOrWhiteSpace(ZipCodeTextBox?.Text))
                parts.Add(ZipCodeTextBox.Text);

            if (!string.IsNullOrWhiteSpace(CountryTextBox?.Text))
                parts.Add(CountryTextBox.Text);

            if (AddressPreviewTextBlock != null)
            {
                AddressPreviewTextBlock.Text = parts.Count > 0 
                    ? string.Join(", ", parts) 
                    : "Enter your address to see preview";
            }
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoading) return;

            if (ThemeComboBox.SelectedItem is ComboBoxItem item && item.Tag != null)
            {
                var theme = Enum.Parse<AppTheme>(item.Tag.ToString()!);
                ThemeChanged?.Invoke(this, theme);
            }
        }

        private void ApiKeyBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // This event is just to acknowledge password changes
            // Actual saving happens in SaveButton_Click
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save theme
            if (ThemeComboBox.SelectedItem is ComboBoxItem themeItem && themeItem.Tag != null)
            {
                _preferences.Theme = Enum.Parse<AppTheme>(themeItem.Tag.ToString()!);
            }

            // Save compact view
            _preferences.UseCompactView = CompactViewCheckBox.IsChecked ?? false;

            // Save show intro on startup
            _preferences.ShowIntroOnStartup = ShowIntroCheckBox.IsChecked ?? true;

            // Save check for updates
            _preferences.CheckForUpdates = CheckForUpdatesCheckBox.IsChecked ?? true;

            // Save suppress warnings
            _preferences.SuppressWarnings = SuppressWarningsCheckBox.IsChecked ?? false;

            // Save auto-save after import
            _preferences.AutoSaveAfterImport = AutoSaveCheckBox.IsChecked ?? false;

            // Save home address fields
            _preferences.Street = StreetTextBox.Text.Trim();
            _preferences.City = CityTextBox.Text.Trim();
            _preferences.State = StateTextBox.Text.Trim();
            _preferences.ZipCode = ZipCodeTextBox.Text.Trim();
            _preferences.Country = CountryTextBox.Text.Trim();

            // Save sort option
            if (SortByComboBox.SelectedItem is ComboBoxItem sortItem)
            {
                _preferences.DefaultSortBy = sortItem.Content?.ToString() ?? "Date Added (Newest)";
            }

            // Save API keys
            _preferences.ClaudeApiKey = ClaudeApiKeyBox.Password;
            _preferences.OpenAiApiKey = OpenAiApiKeyBox.Password;
            _preferences.GeminiApiKey = GeminiApiKeyBox.Password;

            // Save preferred AI provider
            if (PreferredAiProviderComboBox.SelectedItem is ComboBoxItem aiItem && aiItem.Tag != null)
            {
                _preferences.PreferredAiProvider = aiItem.Tag.ToString()!;
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
