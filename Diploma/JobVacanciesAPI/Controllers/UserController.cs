using JobVacanciesAPI.BAL.DTOs.User;
using JobVacanciesAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobVacanciesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("get-user-profile/{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Incorrect user ID: {id}");
            }

            var profile = await _userService.GetUserProfileAsync(id);

            return Ok(profile);

        }

        [HttpPost("edit-recruiter-profile")]
        public async Task<IActionResult> EditRecruiter(RecruiterEditDTO recruiterEdit)
        {
            if (recruiterEdit == null)
            {
                return BadRequest("Model was null");
            }

            await _userService.EditRecruiterProfile(recruiterEdit);

            return Ok();
        }

        [HttpPost("edit-candidate-profile")]
        public async Task<IActionResult> EditCandidate(CandidateEditDTO candidateEdit)
        {
            if (candidateEdit == null)
            {
                return BadRequest("Model was null");
            }

            await _userService.EditCandidateProfile(candidateEdit);

            return Ok();
        }

    }
}
