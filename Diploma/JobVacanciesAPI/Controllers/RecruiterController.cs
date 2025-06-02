using JobVacanciesAPI.BAL.DTOs.Recruiter;
using JobVacanciesAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobVacanciesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecruiterController : Controller
    {
        private readonly IRecruiterService _service;

        public RecruiterController(IRecruiterService service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RecruiterDTO dto)
        {
            if (dto == null) return BadRequest();
            await _service.AddAsync(dto);
            return Ok();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RecruiterDTO dto)
        {
            if (id != dto.Id) return BadRequest();
            await _service.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
