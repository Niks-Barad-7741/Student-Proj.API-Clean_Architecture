using StudentProj.Application.DTOs;

namespace StudentProj.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllAsync();
        Task<DepartmentDTO?> GetByIdAsync(int id);
        Task<DepartmentDTO> CreateAsync(CreateDepartmentDTO dto);
        Task<bool> UpdateAsync(int id, UpdateDepartmentDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
