using System.ComponentModel.DataAnnotations;

namespace CandidateProvider.DTOs;

public class CandidateCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int JobTitleId { get; set; }
    public string Status { get; set; } = "Applied";
    public List<int> SkillIds { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
