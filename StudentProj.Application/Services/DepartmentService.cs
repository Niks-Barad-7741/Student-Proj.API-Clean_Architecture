using AutoMapper;
using StudentProj.Application.DTOs;
using StudentProj.Application.Interfaces;
using StudentProj.Domain.Common;
using StudentProj.Domain.Entities;

namespace StudentProj.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IGenericRepository<Department> _repository;
        private readonly IMapper _mapper;

        public DepartmentService(IGenericRepository<Department> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DepartmentDTO> CreateAsync(CreateDepartmentDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            var department = _mapper.Map<Department>(dto);
            department.IsDeleted = false;
            department.CreatedAt = DateTimeHelper.GetIndianStandardTime();

            var created = await _repository.CreateAsync(department);
            return _mapper.Map<DepartmentDTO>(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetAsync(d => d.Id == id && !d.IsDeleted);
            if (existing == null)
            {
                throw new Exception($"Department with id {id} not found.");
            }

            // Soft delete business logic living in the Service!
            existing.IsDeleted = true;
            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllAsync()
        {
            var departments = await _repository.GetAllByFilterAsync(d => !d.IsDeleted);
            return _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
        }

        public async Task<DepartmentDTO?> GetByIdAsync(int id)
        {
            var department = await _repository.GetAsync(d => d.Id == id && !d.IsDeleted);
            if (department == null) return null;
            return _mapper.Map<DepartmentDTO>(department);
        }

        public async Task<bool> UpdateAsync(int id, UpdateDepartmentDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            var existing = await _repository.GetAsync(d => d.Id == id && !d.IsDeleted);
            if (existing == null)
            {
                throw new Exception($"Department with id {id} not found.");
            }

            existing.Name = dto.Name;
            existing.Description = dto.Description;

            await _repository.UpdateAsync(existing);
            return true;
        }
    }
}
