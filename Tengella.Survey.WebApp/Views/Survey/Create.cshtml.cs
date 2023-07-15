using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tengella.Survey.Data;
namespace Tengella.Survey.WebApp.Views.Survey
{
    public class CreateModel : PageModel
    {
        private readonly SurveyDbContext _surveyDbContext;

        public CreateModel(SurveyDbContext myWorldDbContext)
        {
            _surveyDbContext = myWorldDbContext;
        }

        [BindProperty]
        public Data.Models.Survey Survey { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnPost() {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var entry = _surveyDbContext.Add(new Data.Models.Survey());
            entry.CurrentValues.SetValues(Survey);
            await _surveyDbContext.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}
