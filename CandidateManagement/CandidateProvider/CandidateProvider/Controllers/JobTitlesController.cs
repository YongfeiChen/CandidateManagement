using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CandidateProvider.Data;

namespace CandidateProvider.Controllers;

[ApiController]
[Route("api/[controller]")] // 这里的 [controller] 会自动变为 jobtitles
public class JobTitlesController : ControllerBase
{
    private readonly AppDbContext _context;

    public JobTitlesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetJobTitles()
    {
        var titles = await _context.JobTitles.ToListAsync();
        return Ok(titles);
    }
}
