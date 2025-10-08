using Microsoft.AspNetCore.Mvc;
using Basic_Project.Models;
using Basic_Project.DAL;
using System.Diagnostics;

namespace Basic_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuizRepository _repository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IQuizRepository repository, ILogger<HomeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Loading quiz list");

                var quizzes = await _repository.GetAllQuizzesAsync();

                return View(quizzes);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error loading quiz list");
                return RedirectToAction("Error");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Error page accessed with RequestId: {RequestId}", 
                Activity.Current?.Id ?? HttpContext.TraceIdentifier);

            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}