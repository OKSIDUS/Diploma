using JobVacanciesAPP.BAL.Interfaces;
using JobVacanciesAPP.Models;
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

        [HttpGet]
        public async Task<IActionResult> EditRecruiter()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var profile = await _userService.GetUserProfileAsync(userId);

            var model = new EditRecruiterProfileViewModel
            {
                UserId = userId,
                Email = profile.User.Email,
                Position = profile.Recruiter.Position,
                CompanyName = profile.Recruiter.CompanyName
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditRecruiter([FromForm] EditRecruiterProfileViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model); 
            }
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _userService.EditRecruiterProfile(new DAL.Models.Users.RecruiterEdit
            {
                UserId = userId,
                CompanyName = model.CompanyName,
                Email = model.Email,
                Position = model.Position
            });

            return RedirectToAction("Index");

        }
    }
}
