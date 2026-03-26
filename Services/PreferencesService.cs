using JobSearchTracker.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobSearchTracker.Services
{
    /// <summary>
    /// Service responsible for managing user preferences.
    /// </summary>
    public class PreferencesService
    {
        private readonly string _preferencesFilePath;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreferencesService"/> class.
        /// </summary>
        public PreferencesService()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "JobSearchTracker"
            );

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            _preferencesFilePath = Path.Combine(appDataPath, "preferences.json");
            
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Loads user preferences from disk.
        /// </summary>
        /// <returns>The loaded preferences or default preferences if file doesn't exist.</returns>
        public async Task<UserPreferences> LoadPreferencesAsync()
        {
            if (!File.Exists(_preferencesFilePath))
            {
                return new UserPreferences();
            }

            try
            {
                var json = await File.ReadAllTextAsync(_preferencesFilePath);
                var preferences = JsonSerializer.Deserialize<UserPreferences>(json, _jsonOptions);
                return preferences ?? new UserPreferences();
            }
            catch
            {
                return new UserPreferences();
            }
        }

        /// <summary>
        /// Saves user preferences to disk.
        /// </summary>
        /// <param name="preferences">The preferences to save.</param>
        public async Task SavePreferencesAsync(UserPreferences preferences)
        {
            if (preferences == null)
                throw new ArgumentNullException(nameof(preferences));

            var json = JsonSerializer.Serialize(preferences, _jsonOptions);
            await File.WriteAllTextAsync(_preferencesFilePath, json);
        }
    }
}
