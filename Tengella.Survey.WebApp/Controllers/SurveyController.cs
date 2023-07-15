using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tengella.Survey.Data;
using Tengella.Survey.WebApp.Views.Survey;
using WebApp.Controllers;

namespace Tengella.Survey.WebApp.Controllers
{
    public class SurveyController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly SurveyDbContext _surveyDbcontext;

        [BindProperty]
        public Data.Models.Survey Survey { get; set; }

        public SurveyController(ILogger<HomeController> logger, SurveyDbContext surveyDbcontext)
        {
            _logger = logger;
            _surveyDbcontext = surveyDbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult Create()
        {
            var data = _surveyDbcontext.Surveys
                .Include(Q => Q.Questions)
                .ThenInclude(A => A.Answers)
                .FirstOrDefault();
            return View(data);
            /*var createModel = new CreateModel(_surveyDbcontext);
            return View(createModel);*/
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Data.Models.Survey obj)
        {
            _surveyDbcontext.Add(obj);
            _surveyDbcontext.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            IEnumerable<Data.Models.Survey> surveyList = _surveyDbcontext.Surveys.Where(c => c.Id == 1).ToList();
            return View(surveyList);
        }
    }
}
