using JobSearchTracker.Models;
using JobSearchTracker.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace JobSearchTracker.Views
{
    /// <summary>
    /// Interaction logic for AddJobFromUrlDialog.xaml
    /// </summary>
    public partial class AddJobFromUrlDialog : Window
    {
        private readonly UserPreferences _preferences;
        private Job? _scrapedJob;

        public AddJobFromUrlDialog(UserPreferences preferences)
        {
            InitializeComponent();
            _preferences = preferences ?? throw new ArgumentNullException(nameof(preferences));

            Loaded += (s, e) =>
            {
                // Set the preferred AI provider
                foreach (ComboBoxItem item in AiProviderComboBox.Items)
                {
                    if (item.Tag?.ToString() == _preferences.PreferredAiProvider)
                    {
                        AiProviderComboBox.SelectedItem = item;
                        break;
                    }
                }

                UpdateApiKeyStatus();
            };
        }

        public Job? ScrapedJob => _scrapedJob;

        private void UpdateApiKeyStatus()
        {
            if (AiProviderComboBox == null || ApiKeyStatusTextBlock == null || ScrapeButton == null)
                return;

            var selectedProvider = (AiProviderComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Claude";
            var hasApiKey = selectedProvider switch
            {
                "Claude" => !string.IsNullOrWhiteSpace(_preferences.ClaudeApiKey),
                "OpenAI" => !string.IsNullOrWhiteSpace(_preferences.OpenAiApiKey),
                "Gemini" => !string.IsNullOrWhiteSpace(_preferences.GeminiApiKey),
                _ => false
            };

            ApiKeyStatusTextBlock.Text = hasApiKey 
                ? "✅ API key configured" 
                : "❌ API key not configured - Click 'Configure API Keys'";

            ScrapeButton.IsEnabled = hasApiKey;
        }

        private void AiProviderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateApiKeyStatus();
        }

        private async void ScrapeButton_Click(object sender, RoutedEventArgs e)
        {
            var url = UrlTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                MessageBox.Show("Please enter a valid URL.", "Invalid URL", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedProvider = (AiProviderComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Claude";

            // Create the appropriate AI service
            IAiJobScrapingService? aiService = selectedProvider switch
            {
                "Claude" => new ClaudeJobScrapingService(_preferences.ClaudeApiKey),
                "OpenAI" => new OpenAiJobScrapingService(_preferences.OpenAiApiKey),
                "Gemini" => new GeminiJobScrapingService(_preferences.GeminiApiKey),
                _ => null
            };

            if (aiService == null)
            {
                MessageBox.Show("Failed to initialize AI service.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Disable UI during scraping
            ScrapeButton.IsEnabled = false;
            AiProviderComboBox.IsEnabled = false;
            UrlTextBox.IsEnabled = false;
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.IsIndeterminate = true;

            StatusTextBlock.Text = $"🔄 Scraping job posting using {aiService.ProviderName}...\nThis may take a few moments.";

            try
            {
                var result = await aiService.ScrapeJobFromUrlAsync(url);

                ProgressBar.IsIndeterminate = false;
                ProgressBar.Visibility = Visibility.Collapsed;

                if (result.Success && result.Job != null)
                {
                    _scrapedJob = result.Job;
                    
                    StatusTextBlock.Text = $"✅ Success! Job details extracted:\n\n" +
                        $"Company: {result.Job.CompanyName}\n" +
                        $"Title: {result.Job.JobTitle}\n" +
                        $"Location: {result.Job.Location}\n\n" +
                        $"The job has been added to your project. You can edit it to add more details.";

                    MessageBox.Show(
                        $"Successfully scraped job:\n\n" +
                        $"Company: {result.Job.CompanyName}\n" +
                        $"Title: {result.Job.JobTitle}\n" +
                        $"Location: {result.Job.Location}\n\n" +
                        $"The job has been added to your project!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    DialogResult = true;
                    Close();
                }
                else
                {
                    StatusTextBlock.Text = $"❌ Failed to scrape job:\n{result.ErrorMessage}";

                    if (result.InsufficientCredits)
                    {
                        MessageBox.Show(
                            $"Insufficient API credits or quota exceeded.\n\n" +
                            $"Please check your {aiService.ProviderName} account and ensure you have available credits.",
                            "Insufficient Credits",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Failed to scrape job:\n\n{result.ErrorMessage}",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                ProgressBar.IsIndeterminate = false;
                ProgressBar.Visibility = Visibility.Collapsed;
                StatusTextBlock.Text = $"❌ Unexpected error:\n{ex.Message}";

                MessageBox.Show(
                    $"An unexpected error occurred:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            finally
            {
                // Re-enable UI
                ScrapeButton.IsEnabled = true;
                AiProviderComboBox.IsEnabled = true;
                UrlTextBox.IsEnabled = true;
            }
        }

        private void ConfigureApiKeysButton_Click(object sender, RoutedEventArgs e)
        {
            var preferencesDialog = new PreferencesDialog(_preferences);
            if (preferencesDialog.ShowDialog() == true)
            {
                UpdateApiKeyStatus();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
