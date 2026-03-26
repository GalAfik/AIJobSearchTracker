using System;

namespace JobSearchTracker.Models
{
    /// <summary>
    /// Represents a contact person at a company.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Gets or sets the unique identifier for the contact.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the contact.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the position/title of the contact.
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional notes about the contact.
        /// </summary>
        public string Notes { get; set; } = string.Empty;
    }
}
