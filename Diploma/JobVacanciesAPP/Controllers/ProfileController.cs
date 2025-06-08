using JobVacanciesAPP.BAL.Interfaces;
using JobVacanciesAPP.DAL.Models.Users;
using JobVacanciesAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<IActionResult> EditCandidate()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var profile = await _userService.GetUserProfileAsync(userId);

            var model = new EditCandidateProfileViewModel
            {
                Email = profile.User.Email,
                Skills = profile.Candidate.Skills,
                ResumeFilePath = profile.Candidate.ResumeFilePath,
                DateOfBirth = profile.Candidate.DateOfBirth,
                Experience = profile.Candidate.Experience,
                FullName = profile.Candidate.FullName,
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditCandidate([FromForm] EditCandidateProfileViewModel model)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _userService.EditCandidateProfile(new DAL.Models.Users.CandidateEdit
            {
                UserId = userId,
                FullName = model.FullName,
                ResumeFilePath = model.ResumeFilePath,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                Experience = model.Experience,
                Skills = model.Skills
            });

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> ViewAndEditSkills()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var skills = await _userService.GetCandidateSkills(userId);
            

            var model = new SkillEditViewModel
            {
                AllAvailableTags = skills.AllAvailableTags,
                SelectedTags = skills.SelectedTags
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ViewAndEditSkills(SkillEditViewModel model)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            UserSkills skills = new UserSkills
            {
                UserId = userId,
                SelectedTags = model.SelectedTags,
                AllAvailableTags = model.AllAvailableTags,
            };
            await _userService.SaveCandidateSkills(skills);

            return RedirectToAction("Index");
        }
    }
}
