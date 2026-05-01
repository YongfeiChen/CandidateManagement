using System.ComponentModel.DataAnnotations;

namespace CandidateProvider.DTOs;

/// <summary>
/// Data transfer object for creating a new candidate.
/// </summary>
public class CandidateCreateDto
{
    /// <summary>
    /// Gets or sets the name of the candidate. Required, length between 2 and 50 characters.
    /// Allows Chinese and English characters, spaces, and middle dots.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
    [RegularExpression(@"^[\u4e00-\u9fa5a-zA-Z\s·]+$", ErrorMessage = "Name contains invalid characters. Only Chinese, English, spaces, and middle dots are allowed")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the job title identifier for the candidate.
    /// </summary>
    public int JobTitleId { get; set; }

    /// <summary>
    /// Gets or sets the application status of the candidate (default: "Applied").
    /// </summary>
    public string Status { get; set; } = "Applied";

    /// <summary>
    /// Gets or sets the list of skill identifiers associated with the candidate.
    /// </summary>
    public List<int> SkillIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the date and time when the candidate record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
