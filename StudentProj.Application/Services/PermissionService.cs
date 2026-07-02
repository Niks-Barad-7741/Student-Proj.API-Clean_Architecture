using AutoMapper;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;

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
        public async Task<PermissionDTO> CreatePermissionAsync(PermissionDTO dto)
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
        public async Task<bool> UpdatePermissionRoleAsync(int id, PermissionDTO dto)
        {
            var entity = _mapper.Map<Permissions>(dto);
            return await _repository.UpdatePermissionRoleAsync(id, entity);
        }
    }
}
