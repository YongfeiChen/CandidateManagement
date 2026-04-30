namespace CandidateProvider.Models;

public class JobTitle
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty; // 职位名称，如 ".NET Developer"
    public string Description { get; set; } = string.Empty;

    // 导航属性：一个职位对应多个候选人
    public List<Candidate> Candidates { get; set; } = new();
}
