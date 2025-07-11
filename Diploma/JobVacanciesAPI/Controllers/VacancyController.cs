﻿using JobVacanciesAPI.BAL.DTOs.Vacancy;
using JobVacanciesAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobVacanciesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacancyController : Controller
    {
        private readonly IVacancyService _vacancyService;
        private readonly IApplicationService _applicationService;

        public VacancyController(IVacancyService vacancyService, IApplicationService applicationService)
        {
            _vacancyService = vacancyService;
            _applicationService = applicationService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var vacancies = await _vacancyService.GetAllAsync();
            return Ok(vacancies);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vacancy = await _vacancyService.GetByIdAsync(id);
            if (vacancy == null)
                return NotFound();

            return Ok(vacancy);
        }

        [HttpGet("recruiter/{recruiterId}")]
        public async Task<IActionResult> GetByRecruiter(int recruiterId)
        {
            var vacancies = await _vacancyService.GetByRecruiterAsync(recruiterId);
            return Ok(vacancies);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] VacancyDTO dto)
        {
            dto.CreatedAt = DateTime.UtcNow;
            dto.IsActive = true;

            await _vacancyService.AddAsync(dto);
            return Ok();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VacancyDTO dto)
        {
            if (dto.Id != id)
                return BadRequest("ID не збігається.");

            await _vacancyService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _vacancyService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("{id}/candidates")]
        public async Task<IActionResult> GetCandidatesByVacancy(int id)
        {
            var candidates = await _applicationService.GetCandidatesByVacancyIdAsync(id);
            return Ok(candidates);
        }

        [HttpPost("create-vacancy")]
        public async Task<IActionResult> CreateNewVacancy(CreateVacancyDTO vacancy)
        {
            if(vacancy == null)
            {
                return BadRequest();
            }

            await _vacancyService.CreateVacancy(vacancy);
            return Ok();
        }

        [HttpGet("get-vacancies-page")]
        public async Task<IActionResult> GetVacationPage(int page, int pageSize, bool isRecommendation, int userId, string? keyword,bool mainPage = false)
        {
            var vacancies = await _vacancyService.GetVacancyPage(page, pageSize, isRecommendation, userId,mainPage, keyword);
            return Ok(vacancies);
        }

        [HttpGet("apply-vacancy")]
        public async Task<IActionResult> ApplyVacation(int vacancyId, int userId)
        {
            await _vacancyService.ApplyVacancy(vacancyId, userId);

            return Ok();
        }

        [HttpGet("get-info")]
        public async Task<IActionResult> GetInfo(int id)
        {
            var info = await _vacancyService.GetVacancyInfo(id);
            return Ok(info);
        }

        [HttpGet("get-recruiter-info")]
        public async Task<IActionResult> GetRecruiterInfo(int id)
        {
            var info = await _vacancyService.GetVacancyRecruiter(id);
            return Ok(info);
        }
    }
}
