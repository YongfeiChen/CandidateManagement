namespace CandidateProvider.DTOs;

public class CandidateUpdateDto
{
    public int Id { get; set; } // 更新必须带 ID
    public string Name { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
