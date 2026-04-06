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

            string originalPath = filePath ?? "";

            if (string.IsNullOrEmpty(filePath))
            {
                // Sanitize the project name to create a valid file name
                var sanitizedName = string.Join("_", project.Name.Split(Path.GetInvalidFileNameChars()));

                // Remove leading/trailing spaces and dots (invalid for Windows file names)
                sanitizedName = sanitizedName.Trim(' ', '.');

                // Ensure the name is not empty after sanitization
                if (string.IsNullOrWhiteSpace(sanitizedName))
                {
                    sanitizedName = "Untitled_Project";
                }

                filePath = Path.Combine(_defaultDirectory, $"{sanitizedName}.json");
            }
            else
            {
                // Sanitize the filename portion of the provided path
                var directory = Path.GetDirectoryName(filePath);
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var extension = Path.GetExtension(filePath);

                // Validate that we have a filename
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    throw new ArgumentException($"Invalid file path: '{originalPath}' - filename is empty");
                }

                // Sanitize the filename
                var sanitizedFileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
                sanitizedFileName = sanitizedFileName.Trim(' ', '.');

                // Ensure the filename is not empty after sanitization
                if (string.IsNullOrWhiteSpace(sanitizedFileName))
                {
                    sanitizedFileName = "Untitled_Project";
                }

                // If no extension provided, default to .json
                if (string.IsNullOrEmpty(extension))
                {
                    extension = ".json";
                }

                // Reconstruct the path with sanitized filename
                if (string.IsNullOrEmpty(directory))
                {
                    // No directory specified, use default
                    filePath = Path.Combine(_defaultDirectory, $"{sanitizedFileName}{extension}");
                }
                else
                {
                    filePath = Path.Combine(directory, $"{sanitizedFileName}{extension}");
                }
            }

            // Ensure the directory exists
            var finalDirectory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(finalDirectory))
            {
                if (!Directory.Exists(finalDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(finalDirectory);

                        // Verify directory was actually created
                        if (!Directory.Exists(finalDirectory))
                        {
                            throw new IOException($"Directory was created but is not accessible: {finalDirectory}\n\nThis may be caused by:\n- OneDrive or cloud sync interference\n- Network drive issues\n- Antivirus blocking access\n\nTry saving to Desktop or Documents folder instead.");
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        throw new UnauthorizedAccessException($"No permission to create directory: {finalDirectory}\n\nSolutions:\n1. Use 'Save As' and choose Desktop or Documents\n2. Run as Administrator\n3. Check if this is a OneDrive or network folder", ex);
                    }
                    catch (IOException ex)
                    {
                        throw new IOException($"Failed to create directory: {finalDirectory}\n\nPossible causes:\n- Path is on a network drive that's unavailable\n- OneDrive or cloud sync is blocking access\n- Invalid path or permissions issue\n\nSolution: Use 'Save As' to save to a local folder like Desktop", ex);
                    }
                    catch (Exception ex)
                    {
                        throw new IOException($"Failed to create directory: {finalDirectory}\n\nError: {ex.Message}\n\nTry saving to a different location (Desktop, Documents)", ex);
                    }
                }
                else
                {
                    // Directory exists, verify we can write to it
                    try
                    {
                        string testFile = Path.Combine(finalDirectory, $".test_{Guid.NewGuid()}.tmp");
                        File.WriteAllText(testFile, "test");
                        File.Delete(testFile);
                    }
                    catch (Exception ex)
                    {
                        throw new UnauthorizedAccessException($"Directory exists but cannot write to it: {finalDirectory}\n\nError: {ex.Message}\n\nSolutions:\n1. Check folder permissions\n2. Close any programs accessing this folder\n3. Use 'Save As' to save elsewhere (Desktop)", ex);
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Invalid path: '{filePath}' - could not determine directory");
            }

            // Validate the final path
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Generated file path is empty");
            }

            try
            {
                // Check if file exists and handle read-only attribute
                if (File.Exists(filePath))
                {
                    var fileInfo = new FileInfo(filePath);

                    // Remove read-only attribute if present
                    if (fileInfo.IsReadOnly)
                    {
                        try
                        {
                            fileInfo.IsReadOnly = false;
                        }
                        catch (UnauthorizedAccessException)
                        {
                            throw new UnauthorizedAccessException($"File is read-only and cannot be modified: {filePath}\n\nPlease remove the read-only attribute in File Explorer:\n1. Right-click the file\n2. Select Properties\n3. Uncheck 'Read-only'\n4. Click OK");
                        }
                    }

                    // Check if file is locked by another process (with retry)
                    bool fileLocked = true;
                    int retryCount = 0;
                    const int maxRetries = 3;

                    while (fileLocked && retryCount < maxRetries)
                    {
                        try
                        {
                            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                            {
                                // File is not locked
                                fileLocked = false;
                            }
                        }
                        catch (IOException)
                        {
                            retryCount++;
                            if (retryCount >= maxRetries)
                            {
                                throw new IOException($"File is locked by another program: {filePath}\n\nPossible causes:\n- File is open in Excel, Notepad, or another editor\n- Another instance of Job Search Tracker has it open\n- Antivirus is scanning the file\n\nSolution:\n- Close all programs that might have this file open\n- Wait a few seconds and try again\n- Use 'Save As' to save to a different location");
                            }
                            // Wait before retry
                            await Task.Delay(100);
                        }
                    }
                }

                var json = JsonSerializer.Serialize(project, _jsonOptions);

                // Write with explicit stream management and retry logic
                int writeRetryCount = 0;
                const int maxWriteRetries = 3;
                bool writeSuccess = false;

                while (!writeSuccess && writeRetryCount < maxWriteRetries)
                {
                    try
                    {
                        await File.WriteAllTextAsync(filePath, json);
                        writeSuccess = true;
                    }
                    catch (IOException) when (writeRetryCount < maxWriteRetries - 1)
                    {
                        writeRetryCount++;
                        await Task.Delay(150); // Wait before retry
                    }
                }

                if (!writeSuccess)
                {
                    throw new IOException($"Failed to write to file after {maxWriteRetries} attempts: {filePath}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // More specific error message for permission issues
                var message = $"Access denied to file: {filePath}\n\n";

                if (File.Exists(filePath))
                {
                    var fileInfo = new FileInfo(filePath);
                    message += $"File exists: Yes\n";
                    message += $"Read-only: {fileInfo.IsReadOnly}\n";
                    message += $"Attributes: {fileInfo.Attributes}\n\n";
                    message += "Solutions:\n";
                    message += "1. Close any programs that have this file open\n";
                    message += "2. Check file properties and remove read-only attribute\n";
                    message += "3. Try running as Administrator\n";
                    message += "4. Use 'Save As' to save to a different location";
                }
                else
                {
                    message += "The file doesn't exist yet, but you don't have permission to create it.\n\n";
                    message += "Solutions:\n";
                    message += "1. Choose a different folder (Documents, Desktop)\n";
                    message += "2. Run as Administrator\n";
                    message += "3. Check folder permissions";
                }

                throw new UnauthorizedAccessException(message, ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new DirectoryNotFoundException($"Directory not found: {Path.GetDirectoryName(filePath)}", ex);
            }
            catch (PathTooLongException ex)
            {
                throw new PathTooLongException($"Path too long: {filePath}", ex);
            }
            catch (IOException ex)
            {
                // Provide detailed diagnostic information
                var diagnosticInfo = $"IO error saving to: {filePath}\n\n";
                diagnosticInfo += $"Directory: {Path.GetDirectoryName(filePath)}\n";
                diagnosticInfo += $"File exists: {File.Exists(filePath)}\n";
                diagnosticInfo += $"Directory exists: {Directory.Exists(Path.GetDirectoryName(filePath))}\n\n";

                diagnosticInfo += "Common causes:\n";
                diagnosticInfo += "- File is open in Excel, Notepad, or another program\n";
                diagnosticInfo += "- OneDrive or cloud sync is accessing the file\n";
                diagnosticInfo += "- Antivirus is scanning the file\n";
                diagnosticInfo += "- Network drive connection issue\n";
                diagnosticInfo += "- Insufficient disk space\n\n";

                diagnosticInfo += "Solutions:\n";
                diagnosticInfo += "1. Close ALL programs that might have the file open\n";
                diagnosticInfo += "2. If path is on OneDrive: Pause OneDrive sync temporarily\n";
                diagnosticInfo += "3. Try 'Save As' to Desktop or a different local folder\n";
                diagnosticInfo += "4. Wait 10 seconds and try again\n";
                diagnosticInfo += "5. Restart the application\n\n";

                diagnosticInfo += $"Technical details: {ex.Message}";

                throw new IOException(diagnosticInfo, ex);
            }

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
