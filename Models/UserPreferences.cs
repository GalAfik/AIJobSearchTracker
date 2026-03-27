using JobSearchTracker.ViewModels;

namespace JobSearchTracker.Models
{
    /// <summary>
    /// Represents user preferences for the application.
    /// </summary>
    public class UserPreferences : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the application theme.
        /// </summary>
        public AppTheme Theme { get; set; } = AppTheme.Light;

        /// <summary>
        /// Gets or sets the user's home street address.
        /// </summary>
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's home city.
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's home state/province.
        /// </summary>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's home ZIP/postal code.
        /// </summary>
        public string ZipCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's home country.
        /// </summary>
        public string Country { get; set; } = "United States";

        /// <summary>
        /// Gets the full home address formatted for Google Maps.
        /// </summary>
        public string HomeAddress
        {
            get
            {
                var parts = new List<string>();

                if (!string.IsNullOrWhiteSpace(Street))
                    parts.Add(Street);

                if (!string.IsNullOrWhiteSpace(City))
                    parts.Add(City);

                if (!string.IsNullOrWhiteSpace(State))
                    parts.Add(State);

                if (!string.IsNullOrWhiteSpace(ZipCode))
                    parts.Add(ZipCode);

                if (!string.IsNullOrWhiteSpace(Country))
                    parts.Add(Country);

                return string.Join(", ", parts);
            }
        }

        /// <summary>
        /// Gets or sets the default job sort option.
        /// </summary>
        public string DefaultSortBy { get; set; } = "Date Added (Newest)";

        /// <summary>
        /// Gets or sets the Claude API key.
        /// </summary>
        public string ClaudeApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the OpenAI (ChatGPT) API key.
        /// </summary>
        public string OpenAiApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Google Gemini API key.
        /// </summary>
        public string GeminiApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the preferred AI provider.
        /// </summary>
        public string PreferredAiProvider { get; set; } = "Claude";

        private bool _useCompactView = false;

        /// <summary>
        /// Gets or sets whether to use compact view for job list.
        /// </summary>
        public bool UseCompactView
        {
            get => _useCompactView;
            set => SetProperty(ref _useCompactView, value);
        }

        /// <summary>
        /// Gets or sets whether to show the intro dialog on startup.
        /// </summary>
        public bool ShowIntroOnStartup { get; set; } = true;
    }
}
