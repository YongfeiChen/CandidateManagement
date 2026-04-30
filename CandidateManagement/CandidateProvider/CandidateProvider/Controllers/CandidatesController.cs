using CandidateProvider.Data;
using CandidateProvider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CandidateProvider.DTOs;

namespace CandidateProvider.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CandidatesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper; // 定义 Mapper

    public CandidatesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    // GET: api/candidates (获取所有候选人)
    [HttpGet]
    public async Task<ActionResult<object>> GetCandidates(
    [FromQuery] string? search = null,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 5) // 默认每页 5 个
    {
        var query = _context.Candidates
            .Include(c => c.JobTitle)
            .Include(c => c.Skills)
            .AsQueryable();

        // 1. 搜索逻辑（保留）
        if (!string.IsNullOrWhiteSpace(search))
        {
            string s = search.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(s) ||
                                     (c.JobTitle != null && c.JobTitle.Title.ToLower().Contains(s)));
        }

        // 2. 计算总条数（前端分页控件需要知道一共多少页）
        var totalCount = await query.CountAsync();

        // 3. 执行分页逻辑：跳过之前的页数，只取当前页的大小
        var items = await query
            .OrderByDescending(c => c.CreatedAt) // 通常按创建时间排序，让新人在第一页
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = _mapper.Map<IEnumerable<CandidateReadDto>>(items);

        // 4. 返回一个包含元数据和数据的对象
        return Ok(new
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = dtos
        });
    }

    // POST: api/candidates (添加候选人)
    [HttpPost]
    public async Task<ActionResult<CandidateReadDto>> PostCandidate(CandidateCreateDto createDto)
    {
        var candidate = _mapper.Map<Candidate>(createDto);

        // 根据前端传来的 ID 列表，从数据库加载 Skill 实体
        if (createDto.SkillIds != null && createDto.SkillIds.Count > 0)
        {
            candidate.Skills = await _context.Skills
                .Where(s => createDto.SkillIds.Contains(s.Id))
                .ToListAsync();
        }

        _context.Candidates.Add(candidate);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<CandidateReadDto>(candidate));
    }



    // PUT: api/candidates/1 (修改候选人状态或信息)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCandidate(int id, CandidateUpdateDto updateDto)
    {
        // 1. 验证 ID 是否一致
        if (id != updateDto.Id) return BadRequest("ID 不匹配");

        // 2. 从数据库获取现有实体
        var candidateFromDb = await _context.Candidates.FindAsync(id);
        if (candidateFromDb == null) return NotFound();

        // 3. 核心技巧：使用 AutoMapper 进行“覆盖式映射”
        // 这行代码会把 updateDto 里的值刷进 candidateFromDb
        _mapper.Map(updateDto, candidateFromDb);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Candidates.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent(); // 204
    }



    // DELETE: api/candidates/1 (删除候选人)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCandidate(int id)
    {
        var candidate = await _context.Candidates.FindAsync(id);
        if (candidate == null) return NotFound();

        _context.Candidates.Remove(candidate);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
