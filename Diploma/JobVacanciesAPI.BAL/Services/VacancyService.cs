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
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public VacancyService(IVacancyRepository repo, IMapper mapper, IRecruiterRepository recruiterRepository, ITagRepository tagRepository)
        {
            _repo = repo;
            _mapper = mapper;
            _recruiterRepository = recruiterRepository;
            _tagRepository = tagRepository;
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

        public async Task CreateVacancy(CreateVacancyDTO vacancyDTO)
        {
            if(vacancyDTO != null)
            {
                var recruiter = await _recruiterRepository.GetByIdAsync(vacancyDTO.UserId);

                int vacancyId = await _repo.AddAsync(new Vacancy
                {
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    SalaryMax = vacancyDTO.VacancyInfo.SalaryMax,
                    SalaryMin = vacancyDTO.VacancyInfo.SalaryMin,
                    Description = vacancyDTO.VacancyInfo.Description,
                    Location = vacancyDTO.VacancyInfo.Location,
                    Requirements = vacancyDTO.VacancyInfo.Requirements,
                    Title = vacancyDTO.VacancyInfo.Title,
                    RecruiterId = recruiter.Id
                });

                if (vacancyDTO.Skills != null)
                {
                    var newTags = vacancyDTO.Skills.SelectedTags
                        .Where(tag => !vacancyDTO.Skills.AllAvailableTags.Contains(tag))
                        .ToList();

                    await _tagRepository.SaveNewSkills(newTags);
                    await _tagRepository.SaveVacancySkills(vacancyDTO.Skills.SelectedTags, vacancyId);

                }


            }
        }
    }
}
