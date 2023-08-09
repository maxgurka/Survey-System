using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Tengella.Survey.Data;
using Tengella.Survey.Data.Migrations;
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

		/// <summary>
		/// View for survey creation
		/// </summary>
		/// <param name="id">Optional survey id for using an existing survey as template</param>
		public IActionResult Create(int? id)
		{
			if (id.HasValue)
			{
				Data.Models.Survey? survey = _surveyDbcontext.Surveys?
					.Include(s => s.Questions)!
					.ThenInclude(s => s.Answers)
					.SingleOrDefault(s => s.Id == id);
				if (survey != null)
				{
					return View(survey);
				}
				else
				{
					return NotFound();
				}
			}
			return View();
		}

		/// <summary>
		/// Data from survey creation will be sent here by POST.
		/// </summary>
		/// <param name="jsonData">Json-object containing survey name, description, questions, and optional end date. Questions optionally contains answer alternatives </param>
		[HttpPost]
		public IActionResult Create([FromBody] JsonElement jsonData)
		{
			// Get properties
			if (!jsonData.TryGetProperty("name", out JsonElement nameElement) ||
				!jsonData.TryGetProperty("description", out JsonElement descriptionElement) ||
				!jsonData.TryGetProperty("questions", out JsonElement questionArrayElement) ||
				questionArrayElement.ValueKind != JsonValueKind.Array)
			{
				return BadRequest();
			}

			string surveyName = nameElement.GetString() ?? "";
			string surveyDescription = descriptionElement.GetString() ?? "";

			// Make sure they contain something
			if (string.IsNullOrWhiteSpace(surveyName) || string.IsNullOrWhiteSpace(surveyDescription))
			{
				return BadRequest("Invalid or missing data.");
			}

			// Loop through all questions in the survey
			List<Question> questions = new();
			foreach (JsonElement questionElement in questionArrayElement.EnumerateArray())
			{

				// Get properties
				if (!questionElement.TryGetProperty("name", out JsonElement questionNameElement) ||
					!questionElement.TryGetProperty("answers", out JsonElement answerArrayElement) ||
					answerArrayElement.ValueKind != JsonValueKind.Array)
				{
					return BadRequest("Invalid or missing data.");
				}

				string questionText = questionNameElement.GetString() ?? "";

				// Make sure the string contain something
				if (string.IsNullOrWhiteSpace(questionText))
				{
					return BadRequest();
				}

				// Loop through all answers in the question
				List<Answer> answers = new List<Answer>();
				foreach (JsonElement answerElement in answerArrayElement.EnumerateArray())
				{
					// Get property
					if (!answerElement.TryGetProperty("text", out JsonElement answerTextElement))
					{
						return BadRequest("Invalid or missing data.");
					}

					string answerText = answerTextElement.GetString() ?? "";

					// Make sure the string contain something
					if (string.IsNullOrWhiteSpace(answerText))
					{
						return BadRequest();
					}

					// Add new answer to list
					Answer answer = new Answer
					{
						Content = answerText
					};
					answers.Add(answer);
				}

				// Add new question to list
				Question question = new Question
				{
					Content = questionText,
					Answers = answers
				};
				questions.Add(question);
			}

			// Creating the finished survey
			Data.Models.Survey survey = new Data.Models.Survey
			{
				Name = surveyName,
				Description = surveyDescription,
				Questions = questions
			};

			// Checking for the optional end date
			if (jsonData.TryGetProperty("endDate", out JsonElement endDateElement))
			{
				if (endDateElement.ValueKind == JsonValueKind.String && DateTime.TryParse(endDateElement.GetString(), out DateTime endDate))
				{
					survey.EndDate = endDate;
				}
				else
				{
					return BadRequest("Invalid or missing data");
				}
			}

			_surveyDbcontext.Add(survey);
			_surveyDbcontext.SaveChanges();

			return RedirectToAction("List", "Survey");
		}

		/// <summary>
		/// View for listing all surveys
		/// </summary>
		public IActionResult List()
		{
			// Pass all surveys to the view
			IEnumerable<Data.Models.Survey> surveyList = _surveyDbcontext.Surveys.Include(s => s.Respondents).ToList();
			return View(surveyList);
		}

		/// <summary>
		/// Take survey view
		/// </summary>
		/// <param name="id">Id of survey</param>
		public IActionResult Take(int? id)
		{
			// Find survey with id
			Data.Models.Survey? survey = _surveyDbcontext.Surveys
			.Include(s => s.Questions)!
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

			//TODO: Felhantering
		}

		/// <summary>
		/// Data from a survey (Survey/Take page) filled in by a respondent will be sent here by POST.
		/// </summary>
		/// <param name="jsonData">Json-object containing survey id and answers, each answer containing a question id and an answer string</param>
		[HttpPost]
		public IActionResult Take([FromBody] JsonElement jsonData)
		{
			// Get properties
			int surveyId;
			if (jsonData.TryGetProperty("surveyId", out JsonElement idElement) &&
				jsonData.TryGetProperty("answers", out JsonElement questionArrayElement) &&
				idElement.ValueKind == JsonValueKind.Number &&
				questionArrayElement.ValueKind == JsonValueKind.Array)
			{
				surveyId = idElement.GetInt32();
			}
			else
			{
				return BadRequest();
			}

			// Get response data and add new Response-objects to a list
			List<Response> responses = new();
			foreach (JsonElement answerElement in questionArrayElement.EnumerateArray())
			{
				
				// Getting properties
				if(!answerElement.TryGetProperty("content", out JsonElement contentElement) ||
					!answerElement.TryGetProperty("questionId", out JsonElement questionIdElement))
				{
					return BadRequest();
				}
				int questionId = questionIdElement.GetInt32();
				string questionText = contentElement.GetString() ?? "";

				// Make sure the string contain something
				if (string.IsNullOrWhiteSpace(questionText))
				{
					return BadRequest();
				}

				// Creating the response
				Response response = new Response
				{
					QuestionId = questionId,
					Content = questionText
				};
				responses.Add(response);
			}

			// Create the Respondent object
			Respondent respondent = new Respondent
			{
				Responses = responses
			};

			//Add to survey
			Data.Models.Survey? survey = _surveyDbcontext.Surveys.Include(s => s.Respondents).SingleOrDefault(s => s.Id == surveyId);
			if(survey == null)
			{
				return NotFound();
			}
			survey.Respondents!.Add(respondent);
			_surveyDbcontext.SaveChanges();

			return RedirectToAction("ThankYou", "Survey");
		}

		/// <summary>
		/// "Thank You" view to be shown after respondent has completed a survey
		/// </summary>
		public IActionResult ThankYou()
		{
			return View();
		}

		/// <summary>
		/// Info view, shows information about a survey (for creators/admins, not to be seen by respondents)
		/// </summary>
		/// <param name="id">Id of the survey</param>
		public IActionResult Info(int id)
		{
			// Include all information in the survey
			Data.Models.Survey? survey = _surveyDbcontext.Surveys
				.Include(s => s.Respondents)!
				.ThenInclude(s => s.Responses)
				.Include(s => s.Questions)!
				.ThenInclude(q => q.Answers)
				.Single(s => s.Id == id);

			ViewData["Recipients"] = _surveyDbcontext.Recipients;

			if (survey != null)
			{
			}
			else
			{
				//TODO: felhantering när survey ej existerar
			}


			return View(survey);
		}

		/// <summary>
		/// Post data from email form in the info page
		/// </summary>
		/// <param name="greeting"></param>
		/// <param name="recipientIds"></param>
		/// <param name="message"></param>
		/// <param name="submitType"></param>
		/// <param name="surveyLink"></param>
		[HttpPost]
		public IActionResult Info(string greeting, int[] recipientIds, string message, string submitType, string surveyLink)
		{

			if (submitType == "send")
			{
				// Get the selected recipients from the database based on the IDs
				List<Recipient> recipients = new List<Recipient>();
				foreach (int recipientId in recipientIds)
				{
					Recipient recipient = _surveyDbcontext.Recipients.Single(r => r.Id == recipientId);
					recipients.Add(recipient);
				}
				// Compose the email content for each recipient
				foreach (var recipient in recipients)
				{
					string emailContent = $"{greeting} {recipient.Name},\n\n{message}";

					// Send the email here using recipient.Email as th email
				}
				return Ok();
			}
			else if (submitType == "preview")
			{
				//Return a preview of the message to the first recipient in the list
				Recipient recipient = _surveyDbcontext.Recipients.Single(r => r.Id == recipientIds[0]);
				return Content($"{greeting} {recipient.Name},\n\n{message}\n\n{surveyLink}", "text/plain; charset=utf-8");

			}
			else
			{
				return BadRequest();
			}
		}

		/// <summary>
		/// Delete a survey
		/// </summary>
		/// <param name="id">ID of the survey to be deleted</param>
		public IActionResult Delete(int id)
		{
			// Find the survey with the given id
			var survey = _surveyDbcontext.Surveys.FirstOrDefault(s => s.Id == id);

			if (survey == null)
			{
				return RedirectToAction("List");
			}

			// Remove the survey from the database
			_surveyDbcontext.Surveys.Remove(survey);
			_surveyDbcontext.SaveChanges();

			// Redirect to the survey list page after deletion
			return RedirectToAction("List");
		}
	}
}
