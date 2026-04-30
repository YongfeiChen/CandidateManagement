namespace CandidateProvider.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // 导航属性：关联多个候选人
        public List<Candidate> Candidates { get; set; } = new();
    }

}