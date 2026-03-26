using JobSearchTracker.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobSearchTracker.Services
{
    /// <summary>
    /// Service responsible for saving and loading job search projects to/from JSON files.
    /// </summary>
    public class ProjectService
    {
        private readonly string _defaultDirectory;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectService"/> class.
        /// </summary>
        public ProjectService()
        {
            _defaultDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "JobSearchTracker"
            );

            if (!Directory.Exists(_defaultDirectory))
            {
                Directory.CreateDirectory(_defaultDirectory);
            }

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Saves a job search project to a JSON file.
        /// </summary>
        /// <param name="project">The project to save.</param>
        /// <param name="filePath">The file path where the project should be saved. If null, uses default directory.</param>
        /// <returns>The path where the file was saved.</returns>
        public async Task<string> SaveProjectAsync(JobSearchProject project, string? filePath = null)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            project.DateModified = DateTime.Now;

            if (string.IsNullOrEmpty(filePath))
            {
                var sanitizedName = string.Join("_", project.Name.Split(Path.GetInvalidFileNameChars()));
                filePath = Path.Combine(_defaultDirectory, $"{sanitizedName}.json");
            }

            var json = JsonSerializer.Serialize(project, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);

            return filePath;
        }

        /// <summary>
        /// Loads a job search project from a JSON file.
        /// </summary>
        /// <param name="filePath">The file path of the project to load.</param>
        /// <returns>The loaded project.</returns>
        public async Task<JobSearchProject> LoadProjectAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Project file not found.", filePath);

            var json = await File.ReadAllTextAsync(filePath);
            var project = JsonSerializer.Deserialize<JobSearchProject>(json, _jsonOptions);

            if (project == null)
                throw new InvalidOperationException("Failed to deserialize project.");

            return project;
        }

        /// <summary>
        /// Creates a new job search project.
        /// </summary>
        /// <param name="projectName">The name of the new project.</param>
        /// <param name="description">The description of the project.</param>
        /// <returns>A new project instance.</returns>
        public JobSearchProject CreateNewProject(string projectName, string description = "")
        {
            return new JobSearchProject
            {
                Name = projectName,
                Description = description,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
        }

        /// <summary>
        /// Gets the default directory for storing projects.
        /// </summary>
        public string DefaultDirectory => _defaultDirectory;
    }
}
