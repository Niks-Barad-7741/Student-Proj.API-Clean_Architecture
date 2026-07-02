using AutoMapper;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;

namespace StudentProj.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendenceRepository _repository;
        private readonly IStudent _studentRepository;
        private readonly IMapper _mapper;
        public AttendanceService(IAttendenceRepository repository, IStudent studentRepository, IMapper mapper)
        {
            _repository = repository;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AttendanceDTO>> GetBySubjectIdAsync(int subjectId, DateTime date)
        {
            var entities = await _repository.GetBySubjectIdAsync(subjectId, date);
            return _mapper.Map<IEnumerable<AttendanceDTO>>(entities);
        }
        public async Task<ReportAttendenceDTO> GetRecordAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentbyid(studentId);
            if (student == null) return null;
            var records = await _repository.GetRecordAsync(studentId);
            var recordsList = records.ToList();
            int total = recordsList.Count;
            int present = recordsList.Count(n => n.Status == "Present");
            return new ReportAttendenceDTO
            {
                StudentId = student.Id,
                StudentName = student.Name,
                TotalClasses = total,
                PresentClass = present,
                AttendancePercentage = total == 0 ? 0 : Math.Round((decimal)present / total * 100, 2)
            };
        }
        public async Task<AttendanceDTO> RecordAsync(RecordAttendanceDTO dto)
        {
            var entity = _mapper.Map<Attendance>(dto);
            var saved = await _repository.RecordAsync(entity);
            return _mapper.Map<AttendanceDTO>(saved);
        }
    }
}
