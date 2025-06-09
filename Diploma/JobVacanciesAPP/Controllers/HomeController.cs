using JobVacanciesAPP.BAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobVacanciesAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVacancyService _vacancyService;

        public HomeController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string keyword = "")
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = await _vacancyService.GetVacancyPage(page, 10, userId, false, keyword);
            return View(model);
        }
    }
}
