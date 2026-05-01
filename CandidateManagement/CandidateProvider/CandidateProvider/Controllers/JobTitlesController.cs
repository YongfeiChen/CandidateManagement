using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CandidateProvider.Data;

namespace CandidateProvider.Controllers;

/// <summary>
/// API controller for managing job titles.
/// Provides endpoints for retrieving job title data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class JobTitlesController : ControllerBase
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="JobTitlesController"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public JobTitlesController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all available job titles.
    /// </summary>
    /// <returns>A list of all job titles.</returns>
    [HttpGet]
    public async Task<IActionResult> GetJobTitles()
    {
        var titles = await _context.JobTitles.ToListAsync();
        return Ok(titles);
    }
}
