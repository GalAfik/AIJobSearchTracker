using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JobSearchTracker.Services
{
    /// <summary>
    /// Service for checking software updates from the official website.
    /// </summary>
    public class UpdateService
    {
        private const string UpdateCheckUrl = "https://www.galafik.com/job-search-tracker/";
        private const string CurrentVersion = "0.1.5";

        /// <summary>
        /// Checks if a newer version is available on the website.
        /// </summary>
        /// <returns>The latest version number if an update is available, otherwise null.</returns>
        public async Task<string?> CheckForUpdatesAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                
                var response = await client.GetStringAsync(UpdateCheckUrl);
                
                // Look for version numbers in download links
                // Pattern matches versions like: 0.1.4, 1.0.0, 2.3.4, etc.
                var versionPattern = @"Job[_\s-]?Search[_\s-]?Tracker[_\s-]?v?(\d+\.\d+\.\d+)";
                var matches = Regex.Matches(response, versionPattern, RegexOptions.IgnoreCase);
                
                string? latestVersion = null;
                
                foreach (Match match in matches)
                {
                    if (match.Groups.Count > 1)
                    {
                        var version = match.Groups[1].Value;
                        
                        // Keep track of the highest version found
                        if (latestVersion == null || CompareVersions(version, latestVersion) > 0)
                        {
                            latestVersion = version;
                        }
                    }
                }
                
                // If we found a version and it's newer than current, return it
                if (latestVersion != null && CompareVersions(latestVersion, CurrentVersion) > 0)
                {
                    return latestVersion;
                }
                
                return null;
            }
            catch
            {
                // Silently fail if update check fails (network issues, website down, etc.)
                return null;
            }
        }

        /// <summary>
        /// Compares two version strings (e.g., "0.1.4" vs "0.1.5").
        /// </summary>
        /// <param name="version1">First version string.</param>
        /// <param name="version2">Second version string.</param>
        /// <returns>
        /// &gt; 0 if version1 is greater than version2,
        /// 0 if they are equal,
        /// &lt; 0 if version1 is less than version2.
        /// </returns>
        private int CompareVersions(string version1, string version2)
        {
            try
            {
                var v1 = new Version(version1);
                var v2 = new Version(version2);
                return v1.CompareTo(v2);
            }
            catch
            {
                // If parsing fails, consider versions equal
                return 0;
            }
        }

        /// <summary>
        /// Gets the current application version.
        /// </summary>
        public string GetCurrentVersion()
        {
            return CurrentVersion;
        }
    }
}
