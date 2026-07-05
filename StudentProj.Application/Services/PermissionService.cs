using AutoMapper;
using StudentProj.Application.DTOs;
using StudentProj.Application.Interfaces;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;

namespace StudentProj.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repository;
        private readonly IMapper _mapper;
        public PermissionService(IPermissionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<bool> AssignPermissionToRoleAsync(int roleId, int permissionId, int menuId)
        {
            return await _repository.AssignPermissionToRoleAsync(roleId, permissionId, menuId);
        }
        public async Task<PermissionDTO> CreatePermissionAsync(CreatePermissionDTO dto)
        {
            var entity = _mapper.Map<Permissions>(dto);
            var created = await _repository.CreatePermissionAsync(entity);
            return _mapper.Map<PermissionDTO>(created);
        }
        public async Task<bool> DeletePermissionAsync(int id)
        {
            return await _repository.DeletePermissionAsync(id);
        }
        public async Task<List<PermissionDTO>> GetAllPermissionAsync()
        {
            var entities = await _repository.GetAllPermissionAsync();
            return _mapper.Map<List<PermissionDTO>>(entities);
        }
        public async Task<PermissionDTO> GetPermissionByIdAsync(int id)
        {
            var entity = await _repository.GetPermissionByIdAsync(id);
            return _mapper.Map<PermissionDTO>(entity);
        }
        public async Task<PermissionDTO> GetPermissionByNameAsync(string name)
        {
            var entity = await _repository.GetPermissionByNameAsync(name);
            return _mapper.Map<PermissionDTO>(entity);
        }
        public async Task<List<string>> GetPermissionByRoleIdAsync(List<int> roleIds)
        {
            return await _repository.GetPermissionByRoleIdAsync(roleIds);
        }
        public async Task<List<string>> GetPermissionByRoleNamesAsync(List<string> roleNames)
        {
            return await _repository.GetPermissionByRoleNamesAsync(roleNames);
        }
        public async Task<bool> HasPermissionAsync(int userId, string action, string menuName)
        {
            return await _repository.HasPermissionAsync(userId, action, menuName);
        }
        public async Task<bool> PermissionExistsAsync(string name)
        {
            return await _repository.PermissionExistsAsync(name);
        }
        public async Task<bool> RemovePermissionFromRoleAsync(int roleId, int permissionId, int menuId)
        {
            return await _repository.RemovePermissionFromRoleAsync(roleId, permissionId, menuId);
        }
        public async Task<(bool Success, string Error)> UpdatePermissionRoleAsync(int id, PermissionDTO dto)
        {
            // Check if another permission already has this name
            var existing = await _repository.GetPermissionByNameAsync(dto.PermissionName);
            if (existing != null && existing.Id != id)
                return (false, "A permission with this name already exists");

            var entity = _mapper.Map<Permissions>(dto);
            var result = await _repository.UpdatePermissionRoleAsync(id, entity);
            return (result, result ? null : "Failed to update permission");
        }
    }
}
