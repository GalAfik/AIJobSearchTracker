namespace JobSearchTracker.Models
{
    /// <summary>
    /// Represents the result of an AI job scraping operation.
    /// </summary>
    public class AiJobScrapingResult
    {
        /// <summary>
        /// Gets or sets whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the scraped job data.
        /// </summary>
        public Job? Job { get; set; }

        /// <summary>
        /// Gets or sets an error message if the operation failed.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the error was due to insufficient credits.
        /// </summary>
        public bool InsufficientCredits { get; set; }
    }
}
