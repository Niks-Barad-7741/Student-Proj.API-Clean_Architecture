using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class RoleValidator : AbstractValidator<RoleDTO>
    {
        public RoleValidator()
        {
            RuleFor(x => x.RoleName)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Role name is required!")
                .NotNull()
                .WithMessage("Role name cannot be null!")
                .MinimumLength(3).WithMessage("Role name must be at least 3 characters long")
                .MaximumLength(12)
                .WithMessage("Role name cannot exceed 12 characters!")
                .Matches(@"^[A-Za-z]+(?: [A-Za-z]+)*$")
                .WithMessage("Role name can contain only letters and single spaces")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Role name cannot be the default word 'string'.");
        }
    }

    public class CreateRoleValidator : AbstractValidator<CreateRoleDTO>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.RoleName)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Role name is required!")
                .NotNull()
                .WithMessage("Role name cannot be null!")
                .MinimumLength(3).WithMessage("Role name must be at least 3 characters long")
                .MaximumLength(12)
                .WithMessage("Role name cannot exceed 12 characters!")
                .Matches(@"^[A-Za-z]+(?: [A-Za-z]+)*$")
                .WithMessage("Role name can contain only letters and single spaces")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Role name cannot be the default word 'string'.");
        }
    }
}

