using StudentProj.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProj.Application.Interfaces
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectDTO>> GetAllAsync();
        Task<SubjectDTO> GetByIdAsync(int id);
        Task<SubjectDTO> CreateAsync(CreateSubjectDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<SubjectDTO> UpdateAsync(int id, UpdateSubjectDTO dto);
    }
}
