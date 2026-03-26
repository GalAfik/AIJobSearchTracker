namespace JobSearchTracker.Models
{
    /// <summary>
    /// Represents the current status of a job application.
    /// </summary>
    public enum JobStatus
    {
        NotApplied,
        Applied,
        Interviewed,
        Offered,
        Accepted,
        Rejected,
        Withdrawn
    }
}
