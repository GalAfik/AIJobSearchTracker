using JobSearchTracker.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace JobSearchTracker.Services
{
    /// <summary>
    /// Service responsible for CSV import and export operations.
    /// </summary>
    public class CsvService
    {
        /// <summary>
        /// Exports a job search project to a CSV file.
        /// </summary>
        /// <param name="project">The project to export.</param>
        /// <param name="filePath">The file path where the CSV file should be saved.</param>
        public void ExportToCsv(JobSearchProject project, string filePath)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            var csv = new StringBuilder();

            // Headers
            csv.AppendLine("Company Name,Job Title,Location,Status,Date Posted,Date Applied,Platform,Salary Range,Job URL,Description,Notes");

            // Data
            foreach (var job in project.Jobs.OrderByDescending(j => j.DateAdded))
            {
                csv.AppendLine(string.Join(",",
                    EscapeCsvField(job.CompanyName),
                    EscapeCsvField(job.JobTitle),
                    EscapeCsvField(job.Location),
                    EscapeCsvField(job.Status.ToString()),
                    EscapeCsvField(job.DatePosted?.ToString("MM/dd/yyyy") ?? ""),
                    EscapeCsvField(job.DateApplied?.ToString("MM/dd/yyyy") ?? ""),
                    EscapeCsvField(job.ApplicationPlatform.ToString()),
                    EscapeCsvField(job.SalaryRange),
                    EscapeCsvField(job.JobUrl),
                    EscapeCsvField(job.Description),
                    EscapeCsvField(job.Notes)
                ));
            }

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// Imports jobs from a CSV file and creates a new project.
        /// </summary>
        /// <param name="filePath">The file path of the CSV file to import.</param>
        /// <param name="projectName">The name for the new project.</param>
        /// <param name="projectDescription">The description for the new project.</param>
        /// <returns>A new JobSearchProject with imported jobs.</returns>
        public JobSearchProject ImportFromCsv(string filePath, string projectName, string projectDescription)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("CSV file not found.", filePath);

            var project = new JobSearchProject
            {
                Name = projectName,
                Description = projectDescription,
                DateCreated = DateTime.Now
            };

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            
            if (lines.Length < 2)
                throw new InvalidDataException("CSV file is empty or contains only headers.");

            // Skip header line
            for (int i = 1; i < lines.Length; i++)
            {
                try
                {
                    var fields = ParseCsvLine(lines[i]);
                    
                    if (fields.Count < 2) continue; // Skip empty or invalid lines

                    var job = new Job
                    {
                        CompanyName = GetFieldValue(fields, 0),
                        JobTitle = GetFieldValue(fields, 1),
                        Location = GetFieldValue(fields, 2),
                        Status = ParseEnum<JobStatus>(GetFieldValue(fields, 3), JobStatus.NotApplied),
                        DatePosted = ParseDate(GetFieldValue(fields, 4)),
                        DateApplied = ParseDate(GetFieldValue(fields, 5)),
                        ApplicationPlatform = ParseEnum<ApplicationPlatform>(GetFieldValue(fields, 6), ApplicationPlatform.Other),
                        SalaryRange = GetFieldValue(fields, 7),
                        JobUrl = GetFieldValue(fields, 8),
                        Description = GetFieldValue(fields, 9),
                        Notes = GetFieldValue(fields, 10),
                        DateAdded = DateTime.Now
                    };

                    project.Jobs.Add(job);
                }
                catch
                {
                    // Skip malformed lines
                    continue;
                }
            }

            return project;
        }

        private string EscapeCsvField(string? field)
        {
            if (string.IsNullOrEmpty(field))
                return "\"\"";

            // Escape quotes and wrap in quotes if contains comma, quote, or newline
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }

            return field;
        }

        private List<string> ParseCsvLine(string line)
        {
            var fields = new List<string>();
            var currentField = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        // Escaped quote
                        currentField.Append('"');
                        i++; // Skip next quote
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(currentField.ToString());
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(c);
                }
            }

            fields.Add(currentField.ToString());
            return fields;
        }

        private string GetFieldValue(List<string> fields, int index)
        {
            return index < fields.Count ? fields[index].Trim() : string.Empty;
        }

        private DateTime? ParseDate(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;

            return null;
        }

        private T ParseEnum<T>(string value, T defaultValue) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            if (Enum.TryParse<T>(value, true, out T result))
                return result;

            return defaultValue;
        }
    }
}
