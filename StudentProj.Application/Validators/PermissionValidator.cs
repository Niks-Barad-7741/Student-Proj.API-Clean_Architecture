using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class PermissionValidator : AbstractValidator<PermissionDTO>
    {
        public PermissionValidator()
        {
            RuleFor(x => x.PermissionName)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Permission name is required")
                .NotNull()
                .WithMessage("Permission name cannot be null")
                .Matches(@"^[a-zA-Z\-]+$")
                .WithMessage("Permission must be create, read, update, or delete (with optional suffix like -only)");
        }
    }

    public class CreatePermissionValidator : AbstractValidator<CreatePermissionDTO>
    {
        public CreatePermissionValidator()
        {
            RuleFor(x => x.PermissionName)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Permission name is required")
                .NotNull()
                .WithMessage("Permission name cannot be null")
                .Matches(@"^[a-zA-Z\-]+$")
                .WithMessage("Permission must be create, read, update, or delete (with optional suffix like -only)");
        }
    }
}

