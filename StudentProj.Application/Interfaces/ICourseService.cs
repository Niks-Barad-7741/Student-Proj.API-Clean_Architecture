using StudentProj.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProj.Application.Interfaces
{
    public interface ICourseService
    {
    Task<IEnumerable<CourseDTO>> GetAllAsync();
    Task<CourseDTO> GetByIdAsync(int id);
    Task<CourseDTO> CreateAsync(CreateCourseDTO dto);
    Task<(CourseDTO? Course, string Error)> UpdateAsync(int id, UpdateCourseDTO dto);
    Task<bool> DeleteAsync(int id);

    Task<SubjectDTO> AddSubjectAsync(int courseId, CreateSubjectDTO dto);
    Task<IEnumerable<SubjectDTO>> GetSubjectsAsync(int courseId);
    }
}
