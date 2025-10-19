using Microsoft.AspNetCore.Mvc;
using Planets.Data;
using Planets.ViewModels;
using System.Diagnostics;

namespace Planets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("GenerateTestData")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateTestData()
        {
            await TestDataGenerator.GenerateTestDataAsync(_dbContext);

            return Redirect("/Planets");
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
