using System.ComponentModel.DataAnnotations;

namespace CandidateProvider.DTOs;

public class CandidateCreateDto
{
    [Required(ErrorMessage = "姓名是必填项")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "姓名长度必须在 2 到 50 个字符之间")]
    // 正则解释：允许中文字符、英文字母、空格、以及中间点
    [RegularExpression(@"^[\u4e00-\u9fa5a-zA-Z\s·]+$", ErrorMessage = "姓名包含非法字符，仅限中英文、空格和中间点")]
    public string Name { get; set; } = string.Empty;
    public int JobTitleId { get; set; }
    public string Status { get; set; } = "Applied";
    public List<int> SkillIds { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
