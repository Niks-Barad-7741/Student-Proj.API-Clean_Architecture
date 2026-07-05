using StudentProj.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProj.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<AttendanceDTO> RecordAsync(RecordAttendanceDTO dto);
        Task<IEnumerable<AttendanceDTO>> GetBySubjectIdAsync(int subjectId, DateTime? date);
        Task<ReportAttendenceDTO> GetRecordAsync(int studentId);
    }
}