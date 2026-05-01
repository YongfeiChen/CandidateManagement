namespace CandidateProvider.DTOs
{
    /// <summary>
    /// Data transfer object for reading candidate information.
    /// </summary>
    public class CandidateReadDto
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
        /// Gets or sets the job title identifier for the candidate.
        /// </summary>
        public int JobTitleId { get; set; }

        /// <summary>
        /// Gets or sets the position/job title name of the candidate.
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the application status of the candidate.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of skill names associated with the candidate.
        /// Note: CreatedAt is intentionally excluded to keep the response minimal.
        /// </summary>
        public List<string> SkillNames { get; set; } = new();

    }
}
