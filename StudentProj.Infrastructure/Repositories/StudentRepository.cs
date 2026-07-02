using Microsoft.EntityFrameworkCore;
using StudentProj.Core.Common;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class StudentRepository : IStudent
    {
        private readonly StudentDbcontext _context;
        public StudentRepository(StudentDbcontext context)
        {
            _context = context;
        }
        public async Task<int> Createstudentasync(Student student)
        {
            await _context.Student.AddAsync(student);
            
            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User" && !r.IsDeleted);
            if (userRole != null)
            {
                await _context.StudentRoles.AddAsync(new StudentRoles
                {
                    Student = student,
                    RoleId = userRole.Id
                });
            }

            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteStudentasync(Student student)
        {
            //_context.Student.Remove(student);
            //await _context.SaveChangesAsync();
            //return true;
            student.IsDeleted = true;
            student.DeletedAt = DateTimeHelper.GetIndianStandardTime();
            _context.Student.Update(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAllStudentsasync()
        {
            //return await _context.Student.ToListAsync();
            //   return await _context.Student
            //.Include(x => x.StudentRoles)
            //.ThenInclude(x => x.Role)
            //.ToListAsync();
            return await _context.Student
        .Where(x => !x.IsDeleted)
        .ToListAsync();
        }

        public async Task<Student> GetStudentbyemailasync(string email)
        {
            //return await _context.Student
            //    .Where(s => s.Email.ToLower().Equals(email.ToLower()))
            //    .FirstOrDefaultAsync();
            return await _context.Student
       .Where(x => x.Email.ToLower() == email.ToLower() && !x.IsDeleted)
       .FirstOrDefaultAsync();
        }

        public async Task<Student> GetStudentbyid(int id)
        {
            return await _context.Student.Where(student => student.Id == id && !student.IsDeleted).FirstOrDefaultAsync();
            //    return await _context.Student
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

        public async Task<Student> Getstudentbynameasync(string name)
        {
            return await _context.Student.Where(student => student.Name.ToLower().Contains(name.ToLower()) && !student.IsDeleted).FirstOrDefaultAsync();
            //    return await _context.Student
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
            _context.Student.Update(student);
            await _context.SaveChangesAsync();
            return student.Id == id;
        }

        public async Task<int> UpsertStudentAsync(Student student)
        {
            if (student.Id <= 0)
            {
                await _context.Student.AddAsync(student);
                
                var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User" && !r.IsDeleted);
                if (userRole != null)
                {
                    await _context.StudentRoles.AddAsync(new StudentRoles
                    {
                        Student = student,
                        RoleId = userRole.Id
                    });
                }

                await _context.SaveChangesAsync();
                return student.Id;
            }

            var existingStudent = await _context.Student.FirstOrDefaultAsync(s => s.Id == student.Id && !s.IsDeleted);
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

                _context.Student.Update(existingStudent);
                await _context.SaveChangesAsync();
                return existingStudent.Id;
            }

            return 0;
        }
    }
}
