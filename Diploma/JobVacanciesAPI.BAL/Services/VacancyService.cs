using AutoMapper;
using JobVacanciesAPI.BAL.Entity;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;

namespace JobVacanciesAPI.BAL.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IMapper _mapper;

        public VacancyService(IVacancyRepository vacancyRepository, IMapper mapper)
        {
            _vacancyRepository = vacancyRepository;
            _mapper = mapper;
        }

        public async Task CreateVacancyAsync(VacancyDTO vacancy)
        {
            await _vacancyRepository.CreateAsync(_mapper.Map<Vacancy>(vacancy));
        }

        public async Task DeleteVacancyAsync(int id)
        {
            await _vacancyRepository.DeleteAsync(id);
        }

        public async Task<List<VacancyDTO>> GetAllVacanciesAsync()
        {
            var vacancies = await _vacancyRepository.GetAllAsync();
            return  _mapper.Map<List<VacancyDTO>>(vacancies);
        }

        public async Task<VacancyDTO> GetVacancyAsync(int id)
        {
            var vacancy = await _vacancyRepository.GetByIdAsync(id);
            return _mapper.Map<VacancyDTO>(vacancy);
        }

        public async Task UpdateVacancyAsync(VacancyDTO vacancy)
        {
            await _vacancyRepository.UpdateAsync(_mapper.Map<Vacancy>(vacancy));
        }
    }
}
