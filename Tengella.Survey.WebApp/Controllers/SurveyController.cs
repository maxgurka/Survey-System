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
		private readonly SurveyDbContext _surveyDbcontext;

		public SurveyController(SurveyDbContext surveyDbcontext)
		{
			_surveyDbcontext = surveyDbcontext;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Create()
		{
			return View();
		}

		// POST data from Survey Creation
		[HttpPost]
		public IActionResult Create([FromBody] JsonElement jsonData)
		{
			string surveyName = jsonData.GetProperty("name").GetString();
			string surveyDescription = jsonData.GetProperty("description").GetString();

			// Add all questions to list
            JsonElement questionArrayElement;
			if (jsonData.TryGetProperty("questions", out questionArrayElement) && questionArrayElement.ValueKind == JsonValueKind.Array)
			{
				List<Question> questions = new List<Question>();
				foreach (JsonElement questionElement in questionArrayElement.EnumerateArray())
				{
					string questionName = questionElement.GetProperty("name").GetString();

					// Add all answers to list
					List<Answer> answers = new List<Answer>();
					JsonElement answerArrayElement;
					if (questionElement.TryGetProperty("answers", out answerArrayElement) && answerArrayElement.ValueKind == JsonValueKind.Array)
					{
						foreach (JsonElement answerElement in answerArrayElement.EnumerateArray())
						{
							string answerText = answerElement.GetProperty("text").GetString();
							Answer answer = new Answer {
								Content = answerText
							};
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

				// Creating the survey
				Data.Models.Survey survey = new Data.Models.Survey
				{
					Name = surveyName,
					Description = surveyDescription,
					Questions = questions
				};

				// Checking for optional end-date
                if(jsonData.TryGetProperty("endDate", out JsonElement endDate))
				{
					survey.EndDate = endDate.GetDateTime();
				}

				// Save to database
				_surveyDbcontext.Add(survey);
				_surveyDbcontext.SaveChanges();

				//Send the user to the List view when done
				return RedirectToAction("List", "Survey"); //TODO: give the user feedback after creating survey
			}
			return null; //?
		}

		public IActionResult List()
		{
			// Pass all surveys to the view
			IEnumerable<Data.Models.Survey> surveyList = _surveyDbcontext.Surveys.Include(s => s.Respondents).ToList();
			return View(surveyList);
		}

		public IActionResult Take(int? id)
		{
			// Find survey with id
			Data.Models.Survey? survey = _surveyDbcontext.Surveys
			.Include(s => s.Questions)
			.ThenInclude(q => q.Answers)
			.Single(s => s.Id == id);

			// Check for an end date in survey
            if (survey.EndDate.HasValue)
            {
                DateTime today = DateTime.Today;
                int compareResult = DateTime.Compare(today, survey.EndDate.Value);

                if (compareResult > 0)
                {
					// Survey is no longer open
					return null;
                }
                else if (compareResult <= 0)
                {
                   
                }
            }

            return View(survey);

            //TODO: Felhantering o så
		}

		// Post data from Take survey view
		[HttpPost]
		public IActionResult Take([FromBody] JsonElement jsonData)
		{
			// Get the survey ID
			int surveyId = -1; //
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

		// Info View
		public IActionResult Info(int id)
		{
			// Include all information in the survey
			Data.Models.Survey? survey = _surveyDbcontext.Surveys
				.Include(s => s.Respondents)
				.ThenInclude(s => s.Responses)
				.Include(s => s.Questions)
				.ThenInclude(q => q.Answers)
				.Single(s => s.Id == id);

			if (survey != null)
			{
			}
			else
			{
				//TODO: felhantering när survey ej existerar
			}


			return View(survey);
		}
	}
}
