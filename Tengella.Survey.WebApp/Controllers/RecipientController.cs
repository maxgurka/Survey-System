using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;
using Tengella.Survey.Data;
using Tengella.Survey.Data.Migrations;
using Tengella.Survey.Data.Models;
using Tengella.Survey.WebApp.FileProcessing;
using Tengella.Survey.WebApp.Mapping;

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
		/// View for listing recipients
		/// </summary>
		/// <param name="id">Optional RecipientList id for filtering</param>
		/// <returns></returns>
		public IActionResult List(int? id)
		{
			ViewBag.RecipientLists = _surveyDbcontext.RecipientLists.ToList();
			IEnumerable<Recipient> recipients;

			// Filtering by list id
			if (id != null)
			{
				// Get the recipientList
				RecipientList? recipientList = _surveyDbcontext.RecipientLists
					.Include(r => r.Recipients)
					.SingleOrDefault(r => r.Id == id);

				if (recipientList == null)
				{
					return NotFound();
				}

				// Get recipients in list
				recipients = recipientList.Recipients.ToList();
			}
			else
			{
				recipients = _surveyDbcontext.Recipients.ToList();
			}
			return View(recipients);
		}

		/// <summary>
		/// View information about a recipient
		/// </summary>
		/// <param name="id">The recipient id</param>
		/// <returns></returns>
		public IActionResult Info(int id)
		{
			Recipient recipient = _surveyDbcontext.Recipients.Include(r => r.Respondents).Single(r => r.Id == id);
			if (recipient != null)
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
			if (recipient == null)
			{
				return BadRequest();
			}

			_surveyDbcontext.Recipients.Remove(recipient);
			_surveyDbcontext.SaveChanges();

			return RedirectToAction("List", "Recipient");
		}

		/// <summary>
		/// Creating a new recipient list
		/// </summary>
		/// <param name="listName">The name for the list</param>
		[HttpPost]
		public IActionResult CreateRecipientList(string listName)
		{
			RecipientList recipientList = new() { Name = listName };

			_surveyDbcontext.Add(recipientList);
			_surveyDbcontext.SaveChanges();

			return RedirectToAction("List", "Recipient");
		}

		[HttpPost]
		public IActionResult AddToList(int listId, List<int> recipientIds)
		{
			RecipientList? list = _surveyDbcontext.RecipientLists.Include(r => r.Recipients).SingleOrDefault(r => r.Id == listId);

			if(list == null)
			{
				return NotFound();
			}

			foreach (int id in recipientIds)
			{
				Recipient? recipient = _surveyDbcontext.Recipients.SingleOrDefault(r => r.Id == id);
				if (recipient == null)
				{
					return NotFound();
				}
				list.Recipients!.Add(recipient);
			}
			_surveyDbcontext.SaveChanges();

			return RedirectToAction("List", "Recipient");
		}

		/// <summary>
		/// Add one or more recipients from a file
		/// </summary>
		/// <param name="file">A CSV or an excel file</param>
		[HttpPost]
		public IActionResult AddFromFile(IFormFile file)
		{
			if (file != null && file.Length > 0)
			{
				var recipients = new List<Recipient>();

				using (var streamReader = new StreamReader(file.OpenReadStream()))
				{
					var fileType = Path.GetExtension(file.FileName);
					recipients = FileProcessorFactory.ProcessFile(streamReader.BaseStream, fileType);
				}

				if (recipients.Any())
				{
					_surveyDbcontext.Recipients.AddRange(recipients);
					_surveyDbcontext.SaveChanges();

					return Ok($"Successfully added {recipients.Count} recipients.");
				}
			}

			return BadRequest("No file or empty file was uploaded.");
		}
	}
}
