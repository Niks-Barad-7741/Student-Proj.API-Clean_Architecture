using AutoMapper;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;

namespace StudentProj.Application.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IRegisterRepository _repository;
        private readonly IMapper _mapper;
        public RegisterService(IRegisterRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task AssignRoleAsync(int studentId, int roleId)
        {
            await _repository.AssignRoleAsync(studentId, roleId);
        }
        public async Task<RoleDTO> GetRoleByIdAsync(int roleId)
        {
            var entity = await _repository.GetRoleByIdAsync(roleId);
            return _mapper.Map<RoleDTO>(entity);
        }
        public async Task<StudentDTO> GetStudentByIdAsync(int studentId)
        {
            var entity = await _repository.GetStudentByIdAsync(studentId);
            return _mapper.Map<StudentDTO>(entity);
        }
        public async Task<StudentDTO> GetStudentByPhoneAsync(string phone)
        {
            var entity = await _repository.GetStudentbyphoneasync(phone);
            return _mapper.Map<StudentDTO>(entity);
        }
        public async Task<List<string>> GetStudentRolesAsync(int studentId)
        {
            return await _repository.GetStudentRolesAsync(studentId);
        }
        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            var entity = _mapper.Map<Student>(dto);
            return await _repository.RegisterAsync(entity);
        }
        public async Task<bool> RevokeRoleAsync(int studentId, int roleId)
        {
            return await _repository.RevokeRoleAsync(studentId, roleId);
        }
        public async Task UpdateStudentRoleAsync(int studentId, int roleId)
        {
            await _repository.UpdateStudentRoleAsync(studentId, roleId);
        }
    }
}