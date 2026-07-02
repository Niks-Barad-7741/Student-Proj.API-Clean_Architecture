using AutoMapper;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;

namespace StudentProj.Application.Services
{
    public class StudentService :IStudentService
    {
        private readonly IStudent _repository;
        private readonly IMapper _mapper;
        public StudentService(IStudent repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Createstudentasync(RegisterDTO dto)
        {
            var entity = _mapper.Map<Student>(dto);
            // Hash the password before saving to the database
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            return await _repository.Createstudentasync(entity);
        }

        public async Task<bool> DeleteStudentasync(int id)
        {
            var student = await _repository.GetStudentbyid(id);
            if (student == null)
            {
                return false;
            }
            return await _repository.DeleteStudentasync(student);
        }

        public async Task<List<StudentDTO>> GetAllStudentsasync()
        {
            var entities = await _repository.GetAllStudentsasync();
            return _mapper.Map<List<StudentDTO>>(entities);
        }

        public async Task<StudentDTO> GetStudentbyemailasync(string email)
        {
            var entity = await _repository.GetStudentbyemailasync(email);
            return _mapper.Map<StudentDTO>(entity); 
        }

        public async Task<StudentDTO> GetStudentbyid(int id)
        {
            var entity = await _repository.GetStudentbyid(id);
            return _mapper.Map<StudentDTO>(entity);
        }

        public async Task<StudentDTO> Getstudentbynameasync(string name)
        {
            var entity = await _repository.Getstudentbynameasync(name);
            return _mapper.Map<StudentDTO>(entity);
        }

        public async Task<bool> UpdateStudentasync(int id, StudentDTO dto)
        {
            // Fetch existing so we don't overwrite PasswordHash with null
            var existingEntity = await _repository.GetStudentbyid(id);
            if (existingEntity == null) return false;

            // Update only the fields that are allowed to change
            existingEntity.Name = dto.Name;
            existingEntity.Email = dto.Email;
            existingEntity.Address = dto.Address;
            existingEntity.Phone = dto.Phone;

            return await _repository.UpdateStudentasync(id, existingEntity);
        }

        public async Task<int> UpsertStudentAsync(StudentDTO student)
        {
            var entity = _mapper.Map<Student>(student);
            return await _repository.UpsertStudentAsync(entity);
        }
    }
}
