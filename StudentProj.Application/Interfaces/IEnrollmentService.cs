using StudentProj.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProj.Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentDTO> EnrollStudentAsync(EnrollStudentDTO dto);
        Task<IEnumerable<EnrollmentDTO>> GetStudentByIdAsync(int studentId);
        Task<EnrollmentDTO> UpdateGradeAsync(int id, EnrollmentDTO dto);
    }
}
