using CandidateProvider.Data;
using CandidateProvider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CandidateProvider.DTOs;

namespace CandidateProvider.Controllers;

/// <summary>
/// API controller for managing candidates.
/// Provides endpoints for CRUD operations on candidate records.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CandidatesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CandidatesController"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public CandidatesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all candidates with optional search and pagination support.
    /// </summary>
    /// <param name="search">Optional search query to filter candidates by name or position.</param>
    /// <param name="pageNumber">The page number for pagination (default: 1).</param>
    /// <param name="pageSize">The number of records per page (default: 5).</param>
    /// <returns>A paginated list of candidates with metadata.</returns>
    [HttpGet]
    public async Task<ActionResult<object>> GetCandidates(
    [FromQuery] string? search = null,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 5)
    {
        var query = _context.Candidates
            .Include(c => c.JobTitle)
            .Include(c => c.Skills)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            string s = search.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(s) ||
                                     (c.JobTitle != null && c.JobTitle.Title.ToLower().Contains(s)));
        }

        // Get total count for pagination metadata
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = _mapper.Map<IEnumerable<CandidateReadDto>>(items);

        // Return paginated response with metadata
        return Ok(new
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = dtos
        });
    }

    /// <summary>
    /// Gets candidates filtered by job title and/or skills with pagination support.
    /// </summary>
    /// <param name="jobTitleId">Optional job title identifier to filter candidates.</param>
    /// <param name="skillIds">Optional comma-separated list of skill identifiers to filter candidates.</param>
    /// <param name="pageNumber">The page number for pagination (default: 1).</param>
    /// <param name="pageSize">The number of records per page (default: 5).</param>
    /// <returns>A paginated list of candidates matching the filter criteria with metadata.</returns>
    [HttpGet("filter")]
    public async Task<ActionResult<object>> FilterCandidates(
    [FromQuery] int? jobTitleId = null,
    [FromQuery] string? skillIds = null,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 5)
    {
        var query = _context.Candidates
            .Include(c => c.JobTitle)
            .Include(c => c.Skills)
            .AsQueryable();

        // Filter by job title if specified
        if (jobTitleId.HasValue && jobTitleId > 0)
        {
            query = query.Where(c => c.JobTitleId == jobTitleId.Value);
        }

        // Filter by skills if specified
        if (!string.IsNullOrWhiteSpace(skillIds))
        {
            var skillIdList = skillIds
                .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                .Select(id => int.TryParse(id.Trim(), out var parsedId) ? parsedId : -1)
                .Where(id => id > 0)
                .ToList();

            if (skillIdList.Count > 0)
            {
                // Filter candidates that have ALL specified skills
                query = query.Where(c =>
                    skillIdList.All(skillId =>
                        c.Skills.Any(s => s.Id == skillId)));
            }
        }

        // Get total count for pagination metadata
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = _mapper.Map<IEnumerable<CandidateReadDto>>(items);

        // Return paginated response with metadata
        return Ok(new
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = dtos
        });
    }

    /// <summary>
    /// Creates a new candidate with the provided information.
    /// </summary>
    /// <param name="createDto">The candidate creation data.</param>
    /// <returns>The newly created candidate as a read DTO.</returns>
    [HttpPost]
    public async Task<ActionResult<CandidateReadDto>> PostCandidate(CandidateCreateDto createDto)
    {
        var candidate = _mapper.Map<Candidate>(createDto);

        // Ensure the candidate has a proper creation timestamp
        candidate.CreatedAt = DateTime.UtcNow;

        // Load associated skills from the database
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

        return NoContent();
    }

    /// <summary>
    /// Deletes a candidate with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the candidate to delete.</param>
    /// <returns>No content if successful.</returns>
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
