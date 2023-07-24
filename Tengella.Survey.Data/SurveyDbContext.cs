using Microsoft.EntityFrameworkCore;
namespace Tengella.Survey.Data;

public class SurveyDbContext : DbContext
{
    public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options){}
    public DbSet<Models.Survey> Surveys { get; set; }
    public DbSet<Models.Question> Questions { get; set; }
    public DbSet<Models.Answer> Answers { get; set; }
    public DbSet<Models.Respondent> Respondents { get; set; }
    public DbSet<Models.Response> Responses { get; set; }
}
