using System;
using System.Collections.Generic;

namespace JobSearchTracker.Models
{
    /// <summary>
    /// Represents a job application with all associated details.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Gets or sets the unique identifier for the job.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the company name.
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        public string JobTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the job posting.
        /// </summary>
        public string JobUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the location of the job.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the salary range.
        /// </summary>
        public string SalaryRange { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date when the job was posted.
        /// </summary>
        public DateTime? DatePosted { get; set; }

        /// <summary>
        /// Gets or sets the date when the application was submitted.
        /// </summary>
        public DateTime? DateApplied { get; set; }

        /// <summary>
        /// Gets or sets the platform through which the application was submitted.
        /// </summary>
        public ApplicationPlatform ApplicationPlatform { get; set; } = ApplicationPlatform.Other;

        /// <summary>
        /// Gets or sets the current status of the job application.
        /// </summary>
        public JobStatus Status { get; set; } = JobStatus.NotApplied;

        /// <summary>
        /// Gets or sets the job description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional notes about the job application.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of interviews associated with this job.
        /// </summary>
        public List<Interview> Interviews { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of contacts at the company.
        /// </summary>
        public List<Contact> Contacts { get; set; } = new();

        /// <summary>
        /// Gets or sets the date when the application was rejected (if applicable).
        /// </summary>
        public DateTime? DateRejected { get; set; }

        /// <summary>
        /// Gets or sets the date of offer (if applicable).
        /// </summary>
        public DateTime? DateOffered { get; set; }

        /// <summary>
        /// Gets or sets the date when the job was added to the tracker.
        /// </summary>
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
