using CandidateProvider.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CandidateProvider.Data;

/// <summary>
/// Application database context for Entity Framework Core.
/// Manages the connection to the database and provides access to entity sets.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the DbSet for Candidate entities.
    /// </summary>
    public DbSet<Candidate> Candidates => Set<Candidate>();

    /// <summary>
    /// Gets or sets the DbSet for JobTitle entities.
    /// </summary>
    public DbSet<JobTitle> JobTitles => Set<JobTitle>();

    /// <summary>
    /// Gets or sets the DbSet for Skill entities.
    /// </summary>
    public DbSet<Skill> Skills => Set<Skill>();

    /// <summary>
    /// Configures the model for the database.
    /// </summary>
    /// <param name="modelBuilder">The model builder instance.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ensure correct table name mappings
        modelBuilder.Entity<Candidate>().ToTable("Candidates");
        modelBuilder.Entity<JobTitle>().ToTable("JobTitles");
        modelBuilder.Entity<Skill>().ToTable("Skills");
    }
}
