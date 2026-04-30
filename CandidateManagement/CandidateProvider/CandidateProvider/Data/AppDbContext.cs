using CandidateProvider.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CandidateProvider.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<JobTitle> JobTitles => Set<JobTitle>();
    public DbSet<Skill> Skills => Set<Skill>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 确保表名正确映射
        modelBuilder.Entity<Candidate>().ToTable("Candidates");
        modelBuilder.Entity<JobTitle>().ToTable("JobTitles");
        modelBuilder.Entity<Skill>().ToTable("Skills");
    }
}
