using AutoMapper;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;

namespace StudentProj.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        public RoleService(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<RoleDTO> CreateRoleAsync(RoleDTO dto)
        {
            var entity = _mapper.Map<Roles>(dto);
            var created = await _repository.CreateRoleAsync(entity);
            return _mapper.Map<RoleDTO>(created);
        }
        public async Task<bool> DeleteRoleAsync(int id)
        {
            return await _repository.DeleteRoleAsync(id);
        }
        public async Task<List<RoleDTO>> GetAllRolesAsync()
        {
            var entities = await _repository.GetAllRolesAsync();
            return _mapper.Map<List<RoleDTO>>(entities);
        }
        public async Task<RoleDTO> GetRoleByIdAsync(int id)
        {
            var entity = await _repository.GetRoleByIdAsync(id);
            return _mapper.Map<RoleDTO>(entity);
        }
        public async Task<RoleDTO> GetRoleByNameAsync(string roleName)
        {
            var entity = await _repository.GetRoleByNameAsync(roleName);
            return _mapper.Map<RoleDTO>(entity);
        }
        public async Task<List<string>> GetUserRolesAsync(int studentId)
        {
            return await _repository.GetUserRolesAsync(studentId);
        }
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _repository.RoleExistsAsync(roleName);
        }
        public async Task<bool> UpdateRoleAsync(int id, RoleDTO dto)
        {
            var entity = _mapper.Map<Roles>(dto);
            return await _repository.UpdateRoleAsync(id, entity);
        }
    }
}