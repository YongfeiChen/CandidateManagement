namespace CandidateProvider.DTOs;

public class CandidateUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // 关键：这里要传 ID (int)，而不是 Title (string)
    public int JobTitleId { get; set; }

    public string Status { get; set; } = string.Empty;
}
