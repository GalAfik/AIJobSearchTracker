using JobSearchTracker.Models;
using System.Threading.Tasks;

namespace JobSearchTracker.Services
{
    /// <summary>
    /// Interface for AI services that can scrape job postings.
    /// </summary>
    public interface IAiJobScrapingService
    {
        /// <summary>
        /// Scrapes a job posting from a URL and extracts job details.
        /// </summary>
        /// <param name="url">The URL of the job posting.</param>
        /// <returns>The result of the scraping operation.</returns>
        Task<AiJobScrapingResult> ScrapeJobFromUrlAsync(string url);

        /// <summary>
        /// Validates that the API key is configured and valid.
        /// </summary>
        /// <returns>True if the API key is valid, false otherwise.</returns>
        Task<bool> ValidateApiKeyAsync();

        /// <summary>
        /// Gets the name of the AI provider.
        /// </summary>
        string ProviderName { get; }
    }
}
