namespace CandidateProvider.DTOs;

/// <summary>
/// Data transfer object for updating an existing candidate.
/// </summary>
public class CandidateUpdateDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the candidate to be updated.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the updated name of the candidate.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the job title identifier for the candidate.
    /// Note: Pass the ID (int) instead of the Title (string).
    /// </summary>
    public int JobTitleId { get; set; }

    /// <summary>
    /// Gets or sets the updated application status of the candidate.
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
