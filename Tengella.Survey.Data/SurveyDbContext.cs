using Microsoft.EntityFrameworkCore;
using Tengella.Survey.Data.Models;

namespace Tengella.Survey.Data;

public class SurveyDbContext : DbContext
{
    public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options) { }
    public DbSet<Models.Survey> Surveys { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Respondent> Respondents { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<Recipient> Recipients { get; set; }
	public DbSet<RecipientList> RecipientLists { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Survey>()
            .HasMany(s => s.Questions)
            .WithOne(q => q.Survey)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Models.Survey>()
            .HasMany(s => s.Respondents)
            .WithOne(r => r.Survey)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Question>()
            .HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Respondent>()
            .HasMany(r => r.Responses)
            .WithOne(r => r.Respondent)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Respondent>()
            .HasOne(r => r.Recipient)
            .WithMany(r => r.Respondents)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Recipient>()
            .HasMany(r => r.Respondents)
            .WithOne(r => r.Recipient)
            .OnDelete(DeleteBehavior.SetNull);

		base.OnModelCreating(modelBuilder);
    }
}
