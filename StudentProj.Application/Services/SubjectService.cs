using AutoMapper;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;

namespace StudentProj.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _repository;
        private readonly IMapper _mapper;
        public SubjectService(ISubjectRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<SubjectDTO> CreateAsync(CreateSubjectDTO dto)
        {
            var entity = _mapper.Map<Subject>(dto);
            var created = await _repository.CreateAsync(entity);
            return _mapper.Map<SubjectDTO>(created);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<IEnumerable<SubjectDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SubjectDTO>>(entities);
        }
        public async Task<SubjectDTO> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<SubjectDTO>(entity);
        }
        public async Task<SubjectDTO> UpdateAsync(int id, UpdateSubjectDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;
            _mapper.Map(dto, existing);
            var updated = await _repository.UpdateAsync(id, existing);
            return _mapper.Map<SubjectDTO>(updated);
        }
    }
}
