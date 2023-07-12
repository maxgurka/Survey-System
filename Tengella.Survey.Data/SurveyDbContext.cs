using Tengella.Survey.Data.Models.Example;
using Microsoft.EntityFrameworkCore;
namespace Tengella.Survey.Data;

public class SurveyDbContext : DbContext
{
    public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options)
    {
    }
    public DbSet<Models.Survey> Surveys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seeding a Survey
        modelBuilder.Entity<Models.Survey>().HasData(new Models.Survey
        {
            Id = 1,
            Name = "survey name",
            info = "this is a survey"
        });
    }
}
