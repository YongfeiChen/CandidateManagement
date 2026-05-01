namespace CandidateProvider.Models
{
    /// <summary>
    /// Represents a job candidate in the system.
    /// </summary>
    public class Candidate
    {
        /// <summary>
        /// Gets or sets the unique identifier for the candidate.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the candidate.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the application status of the candidate (e.g., Applied, Interviewing, Hired).
        /// </summary>
        public string Status { get; set; } = "Applied";

        /// <summary>
        /// Gets or sets the foreign key reference to the JobTitle entity.
        /// </summary>
        public int JobTitleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated JobTitle. Can be null if not assigned.
        /// </summary>
        public JobTitle? JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the list of skills associated with this candidate.
        /// </summary>
        public List<Skill> Skills { get; set; } = new();

        /// <summary>
        /// Gets or sets the date and time when the candidate record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
