namespace CandidateProvider.Models
{
    /// <summary>
    /// Represents a skill that can be associated with candidates.
    /// </summary>
    public class Skill
    {
        /// <summary>
        /// Gets or sets the unique identifier for the skill.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the skill.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the navigation property to the list of candidates with this skill.
        /// </summary>
        public List<Candidate> Candidates { get; set; } = new();
    }

}