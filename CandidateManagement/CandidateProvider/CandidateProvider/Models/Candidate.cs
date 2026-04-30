namespace CandidateProvider.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "Applied";

        // 外键字段
        public int JobTitleId { get; set; }

        // 导航属性：这是对象层面的引用，可能为 null
        public JobTitle? JobTitle { get; set; }
        public List<Skill> Skills { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
