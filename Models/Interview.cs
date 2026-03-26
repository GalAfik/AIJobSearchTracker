using System;

namespace JobSearchTracker.Models
{
    /// <summary>
    /// Represents an interview associated with a job application.
    /// </summary>
    public class Interview
    {
        /// <summary>
        /// Gets or sets the unique identifier for the interview.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the interviewer.
        /// </summary>
        public string InterviewerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the interviewer.
        /// </summary>
        public string InterviewerEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time of the interview.
        /// </summary>
        public DateTime? InterviewDateTime { get; set; }

        /// <summary>
        /// Gets or sets the type of interview (e.g., Phone, Video, In-Person, Technical).
        /// </summary>
        public string InterviewType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the round number of the interview (e.g., 1st round, 2nd round).
        /// </summary>
        public int Round { get; set; } = 1;

        /// <summary>
        /// Gets or sets notes about the interview.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the outcome of the interview.
        /// </summary>
        public string Outcome { get; set; } = string.Empty;
    }
}
