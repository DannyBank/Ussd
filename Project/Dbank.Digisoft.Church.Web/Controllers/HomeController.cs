using Dbank.Digisoft.Church.Web.Models;
using Dbank.Digisoft.Church.Web.Services.Authorization;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dbank.Digisoft.Church.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
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