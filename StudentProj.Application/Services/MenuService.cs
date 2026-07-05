using AutoMapper;
using StudentProj.Application.DTOs;
using StudentProj.Application.Interfaces;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;

namespace StudentProj.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repository;
        private readonly IMapper _mapper;
        public MenuService(IMenuRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<MenuDTO> CreateMenuAsync(CreateMenuDTO dto)
        {
            var entity = _mapper.Map<Menu>(dto);
            var created = await _repository.CreateMenuAsync(entity);
            return _mapper.Map<MenuDTO>(created);
        }
        public async Task<bool> DeleteMenuAsync(int id)
        {
            return await _repository.DeleteMenuAsync(id);
        }
        public async Task<List<MenuDTO>> GetAllMenusAsync()
        {
            var entities = await _repository.GetAllMenusAsync();
            return _mapper.Map<List<MenuDTO>>(entities);
        }
        public async Task<MenuDTO> GetMenuByIdAsync(int id)
        {
            var entity = await _repository.GetMenuByIdAsync(id);
            return _mapper.Map<MenuDTO>(entity);
        }
        public async Task<MenuDTO> GetMenuByNameAsync(string name)
        {
            var entity = await _repository.GetMenuByNameAsync(name);
            return _mapper.Map<MenuDTO>(entity);
        }
        public async Task<List<MenuDTO>> GetMenusFromUserAsync(int userId, List<string> roles)
        {
            var entities = await _repository.GetMenusFromUserAsync(userId, roles);
            return _mapper.Map<List<MenuDTO>>(entities);
        }
        public async Task<bool> MenuExistsAsync(string name)
        {
            return await _repository.MenuExistsAsync(name);
        }
        public async Task<(bool Success, string Error)> UpdateMenuAsync(int id, MenuDTO dto)
        {
            // Check if another menu already has this name
            var existing = await _repository.GetMenuByNameAsync(dto.MenuName);
            if (existing != null && existing.Id != id)
                return (false, "A menu with this name already exists");

            var entity = _mapper.Map<Menu>(dto);
            var result = await _repository.UpdateMenuAsync(id, entity);
            return (result, result ? null : "Failed to update menu");
        }
    }
}
