using AutoMapper;
using JobVacanciesAPI.BAL.DTOs.Vacancy;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;

namespace JobVacanciesAPI.BAL.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepository _repo;
        private readonly IMapper _mapper;

        public VacancyService(IVacancyRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<VacancyDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<VacancyDTO>>(list);
        }

        public async Task<VacancyDTO?> GetByIdAsync(int id)
        {
            var vacancy = await _repo.GetByIdAsync(id);
            return vacancy == null ? null : _mapper.Map<VacancyDTO>(vacancy);
        }

        public async Task<List<VacancyDTO>> GetByRecruiterAsync(int recruiterId)
        {
            var list = await _repo.GetByRecruiterIdAsync(recruiterId);
            return _mapper.Map<List<VacancyDTO>>(list);
        }

        public async Task AddAsync(VacancyDTO dto)
        {
            var entity = new Vacancy
            {
                CreatedAt = dto.CreatedAt,
                Description = dto.Description,
                IsActive = dto.IsActive,
                Location = dto.Location,
                RecruiterId = dto.RecruiterId,
                Requirements = dto.Requirements,
                SalaryMax = dto.SalaryMax,
                SalaryMin = dto.SalaryMin,
                Title = dto.Title,
            };
            await _repo.AddAsync(entity);
        }

        public async Task UpdateAsync(VacancyDTO dto)
        {
            var entity = _mapper.Map<Vacancy>(dto);
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
