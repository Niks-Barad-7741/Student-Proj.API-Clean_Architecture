using AutoMapper;
using StudentProj.Application.DTOs;
using StudentProj.Application.Interfaces;
using StudentProj.Domain.Interfaces;

namespace StudentProj.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _repository;
        private readonly IMapper _mapper;
        public LoginService(ILoginRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<StudentDTO> GetStudentbyemailasync(string email)
        {
            var entity = await _repository.GetStudentbyemailasync(email);
            return _mapper.Map<StudentDTO>(entity);
        }
        public async Task<List<string>> GetStudentRolesAsync(int studentId)
        {
            return await _repository.GetStudentRolesAsync(studentId);
        }
        public async Task<List<UserMenuPermissionDTO>> GetStudentPermissionAsync(int studentId)
        {
            var rawPermissions = await _repository.GetStudentPermissionAsync(studentId);
            // Map the raw RolePermissions to the UserMenuPermissionDTO
            var dtos = rawPermissions.Select(p => new UserMenuPermissionDTO
            {
                MenuId = p.Menu.Id,
                MenuName = p.Menu.MenuName,
                MenuRoute = p.Menu.MenuRoute,
                Permission = p.Permission.PermissionName
            })
            .DistinctBy(dto => new { dto.MenuId, dto.Permission }) // Ensure no duplicates
            .ToList();
            return dtos;
        }
    }
}
