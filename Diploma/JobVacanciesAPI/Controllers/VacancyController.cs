using JobVacanciesAPI.BAL.Entity;
using JobVacanciesAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobVacanciesAPI.Controllers
{
    [ApiController]
    [Route("api/")]
    public class VacancyController : Controller
    {
        private readonly IVacancyService _vacancyService;

        public VacancyController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult> GetAll()
        {
            var vacancies = await _vacancyService.GetAllVacanciesAsync();
            return Ok(vacancies);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            } 

            var vacancy = await _vacancyService.GetVacancyAsync(id);
            return Ok(vacancy);
        }

        [HttpPost("create-vacancy")]
        public async Task<IActionResult> Create([FromBody] VacancyDTO vacancy)
        {
            if (vacancy == null)
            {
                return BadRequest("Vacancy is null");
            }

            await _vacancyService.CreateVacancyAsync(vacancy);

            return Ok();
        }


        [HttpPost("update-vacancy")]
        public async Task<IActionResult> Update([FromBody] VacancyDTO vacancy)
        {
            if (vacancy == null)
            {
                return BadRequest("Vacancy is null");
            }

            await _vacancyService.UpdateVacancyAsync(vacancy);
            return Ok();
        }

        [HttpDelete("delete-vacancy/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest("Incorrect ID");
            }

            await _vacancyService.DeleteVacancyAsync(id);
            return Ok();
        }
    }
}
