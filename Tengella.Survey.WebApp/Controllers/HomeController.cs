﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tengella.Survey.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SurveyDbContext _surveyDbcontext;

        public HomeController(ILogger<HomeController> logger, SurveyDbContext surveyDbcontext)
        {
            _logger = logger;
            _surveyDbcontext = surveyDbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}