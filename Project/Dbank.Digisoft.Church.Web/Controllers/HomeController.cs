﻿using AutoMapper;
using Dbank.Digisoft.Church.Web.Models;
using Dbank.Digisoft.Church.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dbank.Digisoft.Church.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChurchMemberService _service;

        public HomeController(ILogger<HomeController> logger, ChurchMemberService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var churches = await _service.GetChurches();
            return View(churches.ToChurchModel());
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