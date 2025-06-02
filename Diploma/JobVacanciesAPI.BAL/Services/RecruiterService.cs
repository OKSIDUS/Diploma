using AutoMapper;
using JobVacanciesAPI.BAL.Entity;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;

namespace JobVacanciesAPI.BAL.Services
{
    public class RecruiterService : IRecruiterService
    {
        private readonly IRecruiterRepository _repo;
        private readonly IMapper _mapper;

        public RecruiterService(IRecruiterRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<RecruiterDTO>> GetAllAsync()
        {
            var recruiters = await _repo.GetAllAsync();
            return _mapper.Map<List<RecruiterDTO>>(recruiters);
        }

        public async Task<RecruiterDTO?> GetByIdAsync(int id)
        {
            var recruiter = await _repo.GetByIdAsync(id);
            return recruiter == null ? null : _mapper.Map<RecruiterDTO>(recruiter);
        }

        public async Task AddAsync(RecruiterDTO dto)
        {
            var entity = new Recruiter
            {
                CompanyName = dto.CompanyName,
                Position = dto.Position,
                UserId = dto.UserId,
            };
            await _repo.AddAsync(entity);
        }

        public async Task UpdateAsync(RecruiterDTO dto)
        {
            var entity = _mapper.Map<Recruiter>(dto);
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
