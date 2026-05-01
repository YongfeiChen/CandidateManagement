namespace CandidateProvider.Models;

/// <summary>
/// Represents a job title in the system.
/// </summary>
public class JobTitle
{
    /// <summary>
    /// Gets or sets the unique identifier for the job title.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title name (e.g., ".NET Developer").
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the job title.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the navigation property to the list of candidates with this job title.
    /// </summary>
    public List<Candidate> Candidates { get; set; } = new();
}
