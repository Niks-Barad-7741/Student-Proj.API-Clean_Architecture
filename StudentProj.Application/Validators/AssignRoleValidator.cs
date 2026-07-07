using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class AssignRoleValidator : AbstractValidator<AssignRoleDTO>
    {
        public AssignRoleValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0)
                .WithMessage("Student Id must be greater than 0");

            RuleFor(x => x.RoleNames)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .NotEmpty()
                .WithMessage("Role Names are required")
                .Matches(@"^[A-Za-z0-9 ]+(?:,[A-Za-z0-9 ]+)*$")
                .WithMessage("Role Names must be a comma-separated list of names (e.g. 'Admin,User')");
        }
    }
}
