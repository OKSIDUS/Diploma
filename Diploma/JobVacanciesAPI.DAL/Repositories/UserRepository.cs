using JobVacanciesAPI.DAL.Context;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacanciesAPI.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IJobVacancyDbContext _context;

        public UserRepository(IJobVacancyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task EditUserEmail(string email, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.Email = email;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> GetUserRole(int userId)
        {
            return await _context.Users.Where(u => u.Id == userId).Select(u => u.Role).FirstOrDefaultAsync();
        }
    }
}
