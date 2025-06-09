using JobVacancies.RecommendationSystem.Services;
using JobVacanciesAPI.BAL.DTOs.Application;
using JobVacanciesAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobVacanciesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : Controller
    {
        private readonly IApplicationService _service;
        private readonly RecommendationService _recommendationService;

        public ApplicationsController(IApplicationService service, RecommendationService recommendationService)
        {
            _service = service;
            _recommendationService = recommendationService;
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ApplicationDTO dto)
        {
            await _service.AddAsync(dto);
            return Ok();
        }

        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
        {
            await _service.UpdateStatusAsync(id, newStatus);
            return Ok();
        }

        [HttpGet("train-model")]
        public async Task<IActionResult> TrainModel()
        {
            try
            {
                 var result = await _recommendationService.GetRecommendedVacancies(32);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("update-status")]
        public async Task<IActionResult> ChangeStatus(int vacancyId, int userId, string status)
        {
            await _service.ChangeStatus(vacancyId, userId, status);
            return Ok();
        }

        [HttpGet("get-user-applications")]
        public async Task<IActionResult> GetUserApplications(int userId)
        {
            var info = await _service.GetUserApplications(userId);
            return Ok(info);
        }
    }
}
