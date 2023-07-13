using Microsoft.AspNetCore.Mvc;
using Tengella.Survey.Data;
using WebApp.Controllers;

namespace Tengella.Survey.WebApp.Controllers
{
    public class SurveyController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly SurveyDbContext _surveyDbcontext;

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
            //TODO: Create new survey
            return View();
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
