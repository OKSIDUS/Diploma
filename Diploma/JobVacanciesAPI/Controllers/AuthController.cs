using JobVacanciesAPI.BAL.DTOs.Auth;
using JobVacanciesAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobVacanciesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            var result = await _auth.RegisterAsync(register);
            if (result == null) return BadRequest("User already exists.");
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _auth.LoginAsync(dto);
            if (result == null) return Unauthorized("Invalid credentials.");
            return Ok(result);
        }
    }
}
