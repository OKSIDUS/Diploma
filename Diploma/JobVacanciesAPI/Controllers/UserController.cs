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

    }
}
