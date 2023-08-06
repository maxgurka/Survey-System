using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tengella.Survey.Data;
using Tengella.Survey.Data.Migrations;
using Tengella.Survey.Data.Models;

namespace Tengella.Survey.WebApp.Controllers
{
	public class RecipientController : Controller
	{
		private readonly SurveyDbContext _surveyDbcontext;

		public RecipientController(SurveyDbContext surveyDbcontext)
		{
			_surveyDbcontext = surveyDbcontext;
		}

		/// <summary>
		/// View for listing all recipients
		/// </summary>
		public IActionResult List()
		{
			IEnumerable<Data.Models.Recipient> recipientList = _surveyDbcontext.Recipients;
			return View(recipientList);
		}

		/// <summary>
		/// View information about a recipient
		/// </summary>
		/// <param name="id">The recipient id</param>
		/// <returns></returns>
		public IActionResult Info(int id)
		{
			Recipient recipient = _surveyDbcontext.Recipients.Single(r => r.Id == id);
			if(recipient != null)
			{
				return View(recipient);
			}
			else
			{
				return BadRequest();
			}
		}

		/// <summary>
		/// View for adding new recipients
		/// </summary>
		public IActionResult Create()
		{
			return View();
		}

		/// <summary>
		/// Takes a recipient with POST and saves it to database
		/// </summary>
		/// <param name="recipient">The recipient created by the user</param>
        [HttpPost]
        public IActionResult Create(Recipient recipient)
        {
            if (ModelState.IsValid)
            {
                _surveyDbcontext.Add(recipient);
                _surveyDbcontext.SaveChanges();

                return RedirectToAction("List", "Recipient");
            }

            return View(recipient);
        }

		/// <summary>
		/// Update a recipient
		/// </summary>
		/// <param name="recipient">The recipient to be updated</param>
		[HttpPost]
		public IActionResult Update(Recipient recipient)
		{
			_surveyDbcontext.Attach(recipient); // Attach the updatedRecipient to the context
			_surveyDbcontext.Entry(recipient).State = EntityState.Modified; // Mark it as Modified

			// Save the changes to the database.
			_surveyDbcontext.SaveChanges();

			// Send the user back to the same page
			return RedirectToAction("Info", "Recipient", new { id = recipient.Id });
		}

		/// <summary>
		/// Delete a recipient
		/// </summary>
		/// <param name="recipient">The recipient to be deleted</param>
		[HttpPost]
		public IActionResult Delete(Recipient recipient)
		{
			if (recipient != null)
			{
				_surveyDbcontext.Recipients.Remove(recipient);
				_surveyDbcontext.SaveChanges();
			}
			else
			{
				
			}

			return RedirectToAction("List", "Recipient");
		}
	}
}
