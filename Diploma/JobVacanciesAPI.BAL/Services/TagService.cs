using AutoMapper;
using JobVacanciesAPI.BAL.DTOs.Tag;
using JobVacanciesAPI.BAL.Interfaces;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.DAL.Interfaces;

namespace JobVacanciesAPI.BAL.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repo;
        private readonly IMapper _mapper;

        public TagService(ITagRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<TagDTO>> GetAllAsync()
        {
            var tags = await _repo.GetAllAsync();
            return _mapper.Map<List<TagDTO>>(tags);
        }

        public async Task AddAsync(TagDTO dto)
        {
            var tag = _mapper.Map<Tag>(dto);
            await _repo.AddAsync(tag);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
