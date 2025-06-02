using BCrypt.Net;
using JobVacanciesAPI.BAL.DTOs.Auth;
using JobVacanciesAPI.BAL.DTOs.Vacancy;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobVacanciesAPI.BAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<AuthResponseDTO?> LoginAsync(LoginDTO login)
        {
            var user = await _userRepository.GetByEmail(login.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                return null;
            }

            return new AuthResponseDTO
            {
                Token = GenerateJwtToken(user),
                Role = user.Role,
                UserId = user.Id,
            };
        }

        public async Task<AuthResponseDTO?> RegisterAsync(RegisterDTO register)
        {
            if (await _userRepository.EmailExistsAsync(register.Email))
            {
                return null;
            }

            var user = new User
            {
                Email = register.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Role = register.Role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddAsync(user);

            return new AuthResponseDTO
            {
                Token = GenerateJwtToken(user),
                Role = user.Role,
                UserId = user.Id
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
