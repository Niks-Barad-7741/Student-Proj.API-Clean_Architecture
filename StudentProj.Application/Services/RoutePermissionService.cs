using AutoMapper;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;

namespace StudentProj.Application.Services
{
    public class RoutePermissionService : IRoutePermissionService
    {
        private readonly IRoutePermissionRepository _repository;
        private readonly IMapper _mapper;
        public RoutePermissionService(IRoutePermissionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<RoutePermissionDTO> CreateRoutePermissionAsync(RoutePermissionDTO dto)
        {
            var entity = _mapper.Map<RoutePermissions>(dto);
            var created = await _repository.CreateRoutePermissionAsync(entity);
            return _mapper.Map<RoutePermissionDTO>(created);
        }
        public async Task<bool> DeleteRoutePermissionAsync(int id)
        {
            return await _repository.DeleteRoutePermissionAsync(id);
        }
        public async Task<List<RoutePermissionDTO>> GetAllRoutePermissionsAsync()
        {
            var entities = await _repository.GetAllRoutePermissionsAsync();
            return _mapper.Map<List<RoutePermissionDTO>>(entities);
        }
        public async Task<RoutePermissionDTO> GetRoutePermissionByIdAsync(int id)
        {
            var entity = await _repository.GetRoutePermissionByIdAsync(id);
            return _mapper.Map<RoutePermissionDTO>(entity);
        }
        public async Task<bool> RoutePermissionExistsAsync(string httpMethod, string pathPattern)
        {
            return await _repository.RoutePermissionExistsAsync(httpMethod, pathPattern);
        }
        public async Task<bool> UpdateRoutePermissionAsync(int id, RoutePermissionDTO dto)
        {
            var entity = _mapper.Map<RoutePermissions>(dto);
            return await _repository.UpdateRoutePermissionAsync(id, entity);
        }
    }
}
