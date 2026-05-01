using CandidateProvider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CandidateProvider.Data;

namespace CandidateProvider.Controllers
{
    /// <summary>
    /// API controller for managing skills.
    /// Provides endpoints for retrieving skill data.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillsController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SkillsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all available skills.
        /// </summary>
        /// <returns>A list of all skills.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
            return await _context.Skills.ToListAsync();
        }

    }
}
