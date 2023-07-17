using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Tengella.Survey.Data;
using Tengella.Survey.Data.Models;
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
        }
        //POST
        [HttpPost]
        public IActionResult Create([FromBody] JsonElement jsonData)
        {
            // Parse the JSON data and extract the required information
            string surveyName = jsonData.GetProperty("name").GetString();

            JsonElement questionArrayElement;
            if (jsonData.TryGetProperty("questions", out questionArrayElement) && questionArrayElement.ValueKind == JsonValueKind.Array)
            {
                List<Question> questions = new List<Question>();

                foreach (JsonElement questionElement in questionArrayElement.EnumerateArray())
                {
                    string questionName = questionElement.GetProperty("name").GetString();

                    JsonElement answerArrayElement;
                    if (questionElement.TryGetProperty("answers", out answerArrayElement) && answerArrayElement.ValueKind == JsonValueKind.Array)
                    {
                        List<Answer> answers = new List<Answer>();

                        foreach (JsonElement answerElement in answerArrayElement.EnumerateArray())
                        {
                            string answerText = answerElement.GetProperty("text").GetString();

                            Answer answer = new Answer { AnswerText = answerText };
                            answers.Add(answer);
                        }

                        Question question = new Question
                        {
                            QuestionText = questionName,
                            Answers = answers
                        };

                        questions.Add(question);
                    }
                }

                // Create the Survey object and save it to the database
                Data.Models.Survey survey = new Data.Models.Survey
                {
                    Name = surveyName,
                    Questions = questions
                };

                _surveyDbcontext.Add(survey);
                _surveyDbcontext.SaveChanges();

                return RedirectToAction("List");
            }
            return null;
        }

        public IActionResult List()
        {
            IEnumerable<Data.Models.Survey> surveyList = _surveyDbcontext.Surveys.ToList();
            //IEnumerable<Data.Models.Survey> surveyList = _surveyDbcontext.Surveys.Where(c => c.Id == 1).ToList(); TODO: sortera på rätt användare
            return View(surveyList);
        }

        public IActionResult Take(int? id)
        {
            /*Data.Models.Survey survey = _surveyDbcontext.Surveys.Where(c => c.Id == id).First()
                .Include(Q => Q.Questions)
                .ThenInclude(A => A.Answers)
                .FirstOrDefault();
            return View(survey);*/

            Data.Models.Survey survey = _surveyDbcontext.Surveys
            .Include(s => s.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefault(c => c.Id == id);

            return View(survey);
        }
    }
}
