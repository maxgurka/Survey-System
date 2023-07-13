using Tengella.Survey.Data.Models.Example;
using Microsoft.EntityFrameworkCore;
namespace Tengella.Survey.Data;

public class SurveyDbContext : DbContext
{
    public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options)
    {
    }
    public DbSet<Models.Survey> Surveys { get; set; }
    public DbSet<Models.Question> Questions { get; set; }
    public DbSet<Models.Answer> Answers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seeding a Survey
        modelBuilder.Entity<Models.Survey>().HasData(new Models.Survey
        {
            Id = 1,
            Name = "survey name"
        });
        // Seeding a Question
        /*modelBuilder.Entity<Models.Survey>().HasData(new Models.Survey
        {
            Id = 1,
            Name = "survey name",
            info = "this is a survey"
        });
        // Seeding an Answer
        modelBuilder.Entity<Models.Survey>().HasData(new Models.Survey
        {
            Id = 1,
            Name = "survey name",
            info = "this is a survey"
        });*/
    }
}
