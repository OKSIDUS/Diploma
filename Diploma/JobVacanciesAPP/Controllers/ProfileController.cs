using JobVacanciesAPP.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobVacanciesAPP.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null || userId < 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            var profile = await _userService.GetUserProfileAsync(userId);

            if (profile.User.Role == "Candidate")
            {
                return View("Candidate", profile);
            }
            else if (profile.User.Role == "Recruiter")
            {
                return View("Recruiter", profile);
            }
            
            return View(profile);
        }
    }
}
