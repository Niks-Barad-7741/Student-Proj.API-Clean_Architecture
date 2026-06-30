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
            var entity = _mapper.Map<Student>(dto);
            entity.Id = id;
            return await _repository.UpdateStudentasync(id,entity);

        }

        public async Task<int> UpsertStudentAsync(StudentDTO student)
        {
            var entity = _mapper.Map<Student>(student);
            return await _repository.UpsertStudentAsync(entity);
        }
    }
}
