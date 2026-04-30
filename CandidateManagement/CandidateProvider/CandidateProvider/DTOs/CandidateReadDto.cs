namespace CandidateProvider.DTOs
{
    public class CandidateReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        // 注意：我们故意不放 CreatedAt，假设我们不想让前端知道这条记录是什么时候创建的
        public List<string> SkillNames { get; set; } = new();

    }
}
