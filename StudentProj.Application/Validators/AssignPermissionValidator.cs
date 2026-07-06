using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class AssignPermissionValidator : AbstractValidator<AssignPermissionDTO>
    {
        public AssignPermissionValidator()
        {
            RuleFor(x => x.RoleName)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotNull().WithMessage("Role name cannot be null")
                .NotEmpty().WithMessage("Role name is required");

            RuleFor(x => x.PermissionNames)
                .NotNull().WithMessage("Permission names cannot be null")
                .NotEmpty().WithMessage("Permission names are required")
                .Matches(@"^[a-zA-Z]+(,\s*[a-zA-Z]+)*$")
                .WithMessage("Permission names must be a comma-separated list of words (e.g. 'create, read')");

            RuleFor(x => x.MenuName)
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotNull().WithMessage("Menu name cannot be null")
                .NotEmpty().WithMessage("Menu name is required");
        }
    }
}

