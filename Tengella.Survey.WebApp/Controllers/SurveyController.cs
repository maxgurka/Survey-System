using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Tengella.Survey.Data;
using Tengella.Survey.Data.Models;
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
			string surveyName = jsonData.GetProperty("name").GetString();
			string surveyDescription = jsonData.GetProperty("description").GetString();

			JsonElement questionArrayElement;
			if (jsonData.TryGetProperty("questions", out questionArrayElement) && questionArrayElement.ValueKind == JsonValueKind.Array)
			{
				List<Question> questions = new List<Question>();

				foreach (JsonElement questionElement in questionArrayElement.EnumerateArray())
				{
					string questionName = questionElement.GetProperty("name").GetString();

					List<Answer> answers = new List<Answer>();
					JsonElement answerArrayElement;
					if (questionElement.TryGetProperty("answers", out answerArrayElement) && answerArrayElement.ValueKind == JsonValueKind.Array)
					{

						foreach (JsonElement answerElement in answerArrayElement.EnumerateArray())
						{
							string answerText = answerElement.GetProperty("text").GetString();

							Answer answer = new Answer { Content = answerText };
							answers.Add(answer);
						}

					}
					Question question = new Question
					{
						Content = questionName,
						Answers = answers
					};
					questions.Add(question);
				}

				Data.Models.Survey survey = new Data.Models.Survey
				{
					Name = surveyName,
					Description = surveyDescription,
					Questions = questions
				};

				_surveyDbcontext.Add(survey);
				_surveyDbcontext.SaveChanges();

				return RedirectToAction("List", "Survey");
			}
			return null;
		}

		public IActionResult List()
		{
			IEnumerable<Data.Models.Survey> surveyList = _surveyDbcontext.Surveys.ToList();
			return View(surveyList);
		}

		public IActionResult Take(int? id)
		{
			Data.Models.Survey survey = _surveyDbcontext.Surveys
			.Include(s => s.Questions)
			.ThenInclude(q => q.Answers)
			.FirstOrDefault(c => c.Id == id);

			return View(survey);
		}

		[HttpPost]
		public IActionResult Take([FromBody] JsonElement jsonData)
		{

			// Get the array of answers from the JSON data
			List<Response> responses = new();

			// Loop through each answer in the JSON array
			foreach (JsonElement answerElement in jsonData.EnumerateArray())
			{
				// Get the question ID and content from each answer
				string questionId = answerElement.GetProperty("questionId").ToString();
				string content = answerElement.GetProperty("content").GetString();

				// Create the Answer object and add it to the list of answers
				Response response = new Response
				{
					QuestionId = Int32.Parse(questionId),
					Content = content
				};
				responses.Add(response);
			}

			// Create the Respondent object and set its answers
			Respondent respondent = new Respondent
			{
				Responses = responses
			};

			// Save the Respondent object to the database
			_surveyDbcontext.Add(respondent);
			_surveyDbcontext.SaveChanges();

			return RedirectToAction("ThankYou", "Survey");

		}

		public IActionResult ThankYou()
		{
			return View();
		}

		public IActionResult Info(int? id)
		{
			Data.Models.Survey survey = _surveyDbcontext.Surveys
			.Include(s => s.Questions)
			.ThenInclude(q => q.Answers)
			.FirstOrDefault(c => c.Id == id); //TODO: skicka med relevant info och visa i Info.cshtml

			return View(survey);
		}
	}
}
