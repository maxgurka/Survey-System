using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			Data.Models.Survey? survey = _surveyDbcontext.Surveys
			.Include(s => s.Questions)
			.ThenInclude(q => q.Answers)
			.Single(s => s.Id == id);

			return View(survey);
		}

		[HttpPost]
		public IActionResult Take([FromBody] JsonElement jsonData)
		{
			// Get the survey ID
			int surveyId = -1;
			if (jsonData.TryGetProperty("surveyId", out JsonElement id))
				surveyId = id.GetInt32();

			// Get the responses
			JsonElement questionArrayElement;
			jsonData.TryGetProperty("answers", out questionArrayElement);

			//TODO: Felhantering

			// Get response data and add new Response-objects to a list
			List<Response> responses = new();
			foreach (JsonElement answerElement in questionArrayElement.EnumerateArray())
			{
				string questionId = answerElement.GetProperty("questionId").ToString();
				string content = answerElement.GetProperty("content").GetString();

				Response response = new Response
				{
					QuestionId = Int32.Parse(questionId),
					Content = content
				};
				responses.Add(response);
			}

			// Create the Respondent object
			Respondent respondent = new Respondent
			{
				Responses = responses
			};

			//Add to survey
			Data.Models.Survey survey = _surveyDbcontext.Surveys.Include(s => s.Respondents).Single(s => s.Id == surveyId);
			survey.Respondents.Add(respondent);
			_surveyDbcontext.SaveChanges();

			return RedirectToAction("ThankYou", "Survey");
		}

		public IActionResult ThankYou()
		{
			return View();
		}

		public IActionResult Info(int id)
		{
			Data.Models.Survey? survey = _surveyDbcontext.Surveys
				.Include(s => s.Respondents)
				.ThenInclude(s => s.Responses)
				.Include(s => s.Questions)
				.ThenInclude(q => q.Answers)
				.Single(s => s.Id == id);
			if (survey != null)
			{
				//ViewData["Survey"] = survey; //TODO: Databasen ska uppdateras (migration). Se till att allt skapas rätt från början nu, respondents ska läggas till i existerande surverys. Skicka med rätt saker i denna metod


			}
			else
			{
				// Handle the case where the survey with the given id is not found.
				// You might want to redirect to an error page or display an appropriate message.
			}


			return View(survey);
		}
	}
}
