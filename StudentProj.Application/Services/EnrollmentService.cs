using AutoMapper;
using StudentProj.Application.DTOs;
using StudentProj.Application.Interfaces;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;

namespace StudentProj.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repository;
        private readonly IMapper _mapper;
        public EnrollmentService(IEnrollmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<EnrollmentDTO> EnrollStudentAsync(EnrollStudentDTO dto)
        {
            var entity = _mapper.Map<Enrollment>(dto);
            var saved = await _repository.EnrollStudentAsync(entity);
            return _mapper.Map<EnrollmentDTO>(saved);
        }
        public async Task<IEnumerable<EnrollmentDTO>> GetStudentByIdAsync(int studentId)
        {
            var entities = await _repository.GetStudentByIdAsync(studentId);
            return _mapper.Map<IEnumerable<EnrollmentDTO>>(entities);
        }
        public async Task<EnrollmentDTO> UpdateGradeAsync(int id, UpdateGradeDTO dto)
        {
            var entity = new Enrollment { Grade = dto.Grade };
            var saved = await _repository.UpdateGradeAsync(id, entity);
            return _mapper.Map<EnrollmentDTO>(saved);
        }
    }
}
