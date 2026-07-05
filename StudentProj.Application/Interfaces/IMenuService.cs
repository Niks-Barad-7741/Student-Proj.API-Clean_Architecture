using StudentProj.Application.DTOs;

namespace StudentProj.Application.Interfaces
{
    public interface IMenuService
    {
        Task<List<MenuDTO>> GetAllMenusAsync();
        Task<MenuDTO> GetMenuByIdAsync(int id);
        Task<MenuDTO> GetMenuByNameAsync(string name);
        Task<bool> MenuExistsAsync(string name);
        Task<MenuDTO> CreateMenuAsync(CreateMenuDTO dto);
        Task<(bool Success, string Error)> UpdateMenuAsync(int id, MenuDTO dto);
        Task<bool> DeleteMenuAsync(int id);
        Task<List<MenuDTO>> GetMenusFromUserAsync(int userId, List<string> roles);

    }
}
