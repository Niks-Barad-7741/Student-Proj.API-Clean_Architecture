using Microsoft.EntityFrameworkCore;
using StudentProj.Domain.Common;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudent
    {
        public StudentRepository(StudentDbcontext context) : base(context)
        {
        }
        public async Task<int> Createstudentasync(Student student)
        {
            await _dbContext.Student.AddAsync(student);
            
            var userRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == "User" && !r.IsDeleted);
            if (userRole != null)
            {
                await _dbContext.StudentRoles.AddAsync(new StudentRoles
                {
                    Student = student,
                    RoleId = userRole.Id
                });
            }

            await _dbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteStudentasync(Student student, int? deletedBy = null)
        {
            //_dbContext.Student.Remove(student);
            //await _dbContext.SaveChangesAsync();
            //return true;
            student.IsDeleted = true;
            student.DeletedAt = DateTimeHelper.GetIndianStandardTime();
            if (deletedBy.HasValue)
            {
                student.DeletedBy = deletedBy.Value.ToString();
            }
            _dbContext.Student.Update(student);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAllStudentsasync()
        {
            //return await _dbContext.Student.ToListAsync();
            //   return await _dbContext.Student
            //.Include(x => x.StudentRoles)
            //.ThenInclude(x => x.Role)
            //.ToListAsync();
            return await _dbContext.Student
        .Where(x => !x.IsDeleted)
        .ToListAsync();
        }

        public async Task<Student> GetStudentbyemailasync(string email)
        {
            //return await _dbContext.Student
            //    .Where(s => s.Email.ToLower().Equals(email.ToLower()))
            //    .FirstOrDefaultAsync();
            return await _dbContext.Student
       .Where(x => x.Email.ToLower() == email.ToLower() && !x.IsDeleted)
       .FirstOrDefaultAsync();
        }

        public async Task<Student> GetStudentbyid(int id)
        {
            return await _dbContext.Student.Where(student => student.Id == id && !student.IsDeleted).FirstOrDefaultAsync();
            //    return await _dbContext.Student
            //.Where(x => x.Id == id)
            //.Select(x => new StudentDTO
            //{
            //    Name = x.Name,
            //    Email = x.Email,
            //    Address = x.Address,
            //    Phone = x.Phone
            //})
            //.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Student>> Getstudentbynameasync(string name)
        {
            return await _dbContext.Student.Where(student => student.Name.ToLower().Contains(name.ToLower()) && !student.IsDeleted).ToListAsync();
            //    return await _dbContext.Student
            //.Where(x => x.Name.ToLower().Contains(name.ToLower()))
            //.Select(x => new StudentDTO
            //{
            //    Name = x.Name,
            //    Email = x.Email,
            //    Address = x.Address,
            //    Phone = x.Phone
            //})
            //.FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateStudentasync(int id, Student student)
        {
            // If the student is already tracked (e.g. fetched by the service), we just need to save changes.
            // Using .Update() on an already tracked entity is usually fine in EF Core, but if it causes issues,
            // we can just call SaveChanges. We'll use Update to be safe if it's untracked.
            _dbContext.Student.Update(student);
            await _dbContext.SaveChangesAsync();
            return student.Id == id;
        }

        public async Task<int> UpsertStudentAsync(Student student)
        {
            if (student.Id <= 0)
            {
                await _dbContext.Student.AddAsync(student);
                
                var userRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == "User" && !r.IsDeleted);
                if (userRole != null)
                {
                    await _dbContext.StudentRoles.AddAsync(new StudentRoles
                    {
                        Student = student,
                        RoleId = userRole.Id
                    });
                }

                await _dbContext.SaveChangesAsync();
                return student.Id;
            }

            var existingStudent = await _dbContext.Student.FirstOrDefaultAsync(s => s.Id == student.Id && !s.IsDeleted);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.Email = student.Email;
                existingStudent.Address = student.Address;
                existingStudent.Phone = student.Phone;
                existingStudent.UpdatedAt = student.UpdatedAt;
                existingStudent.UpdatedBy = student.UpdatedBy;

                // Only set password hash if it was provided (new inserts handled above, but just in case)
                if (!string.IsNullOrEmpty(student.PasswordHash))
                {
                    existingStudent.PasswordHash = student.PasswordHash;
                }

                _dbContext.Student.Update(existingStudent);
                await _dbContext.SaveChangesAsync();
                return existingStudent.Id;
            }

            return 0;
        }

        public async Task<Student> GetStudentByPhoneAsync(string phone)
        {
            return await _dbContext.Student
                .Where(s => s.Phone == phone && !s.IsDeleted)
                .FirstOrDefaultAsync();
        }
    }
}
