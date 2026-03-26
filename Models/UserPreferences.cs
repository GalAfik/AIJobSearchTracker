namespace JobSearchTracker.Models
{
    /// <summary>
    /// Represents user preferences for the application.
    /// </summary>
    public class UserPreferences
    {
        /// <summary>
        /// Gets or sets the application theme.
        /// </summary>
        public AppTheme Theme { get; set; } = AppTheme.Light;

        /// <summary>
        /// Gets or sets the user's home address for directions.
        /// </summary>
        public string HomeAddress { get; set; } = string.Empty;

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
    }
}
