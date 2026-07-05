using AutoMapper;
using StudentProj.Application.DTOs;
using StudentProj.Application.Interfaces;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;

namespace StudentProj.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IMapper _mapper;
        public CourseService(ICourseRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<SubjectDTO> AddSubjectAsync(int courseId, CreateSubjectDTO dto)
        {
            var entity = _mapper.Map<Subject>(dto);
            entity.CourseId = courseId;
            var created = await _repository.AddSubjectAsync(courseId, entity);
            return _mapper.Map<SubjectDTO>(created);
        }
        public async Task<CourseDTO> CreateAsync(CreateCourseDTO dto)
        {
            var entity = _mapper.Map<Course>(dto);
            var created = await _repository.CreateAsync(entity);
            return _mapper.Map<CourseDTO>(created);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<IEnumerable<CourseDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseDTO>>(entities);
        }
        public async Task<CourseDTO> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<CourseDTO>(entity);
        }
        public async Task<IEnumerable<SubjectDTO>> GetSubjectsAsync(int courseId)
        {
            var entities = await _repository.GetSubjectsAsync(courseId);
            return _mapper.Map<IEnumerable<SubjectDTO>>(entities);
        }
        public async Task<(CourseDTO? Course, string Error)> UpdateAsync(int id, UpdateCourseDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return (null, "Course not found");

            // Check if another course already has this name
            var nameOwner = await _repository.GetByNameAsync(dto.CourseName);
            if (nameOwner != null && nameOwner.Id != id) return (null, "A course with this name already exists");

            _mapper.Map(dto, existing);
            var updated = await _repository.UpdateAsync(id, existing);
            return (_mapper.Map<CourseDTO>(updated), null);
        }
    }
}
