using System;
using System.Collections.Generic;

namespace JobSearchTracker.Models
{
    /// <summary>
    /// Represents a job search project containing multiple job applications.
    /// </summary>
    public class JobSearchProject
    {
        /// <summary>
        /// Gets or sets the unique identifier for the project.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the project (e.g., "2026 Software Engineer Search").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the project.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date when the project was created.
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the date when the project was last modified.
        /// </summary>
        public DateTime DateModified { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the list of jobs in this project.
        /// </summary>
        public List<Job> Jobs { get; set; } = new();
    }
}
