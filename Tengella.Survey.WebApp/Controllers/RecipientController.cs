using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tengella.Survey.Data;
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
    }
}
