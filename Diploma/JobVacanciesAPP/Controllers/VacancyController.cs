﻿using JobVacanciesAPP.BAL.Interfaces;
using JobVacanciesAPP.DAL.Models.Vacancy;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobVacanciesAPP.Controllers
{
    public class VacancyController : Controller
    {
        private readonly IVacancyService _vacancyService;

        public VacancyController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateVacancy();

            model.Skills = await _vacancyService.GetAllSkills();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVacancy vacancy)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            vacancy.UserId = userId;
            await _vacancyService.CreateVacancy(vacancy);

            return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> Apply(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _vacancyService.VacancyApply(id, userId);

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var info = await _vacancyService.GetVacancyInfo(id);
            return View(info);
        }
        [HttpGet]
        public async Task<IActionResult> ShortDetails(int id)
        {
            var info = await _vacancyService.GetVacancyInfo(id);
            return View(info);
        }

        [HttpGet]
        public async Task<IActionResult> RecruiterDetails(int id)
        {
            var info = await _vacancyService.GetVacancyRecruiter(id);
            return View(info);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptApplication(int id, int candidateId)
        {
            await _vacancyService.ChangeStatus(id, candidateId, "Accept");
            return RedirectToAction("RecruiterDetails", new {id = id});
        }

        [HttpPost]
        public async Task<IActionResult> RejectApplication(int id, int candidateId)
        {
            await _vacancyService.ChangeStatus(id, candidateId, "Decline");
            return RedirectToAction("RecruiterDetails", new {id =id});
        }

        [HttpGet]
        public async Task<IActionResult> Applications()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var info = await _vacancyService.GetCandidateApplications(userId);
            return View(info);
        }
    }
}
