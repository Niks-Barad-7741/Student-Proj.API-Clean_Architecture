using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class AssignPermissionValidator : AbstractValidator<AssignPermissionDTO>
    {
        public AssignPermissionValidator()
        {
            RuleFor(x => x.RoleName)
                .NotNull().WithMessage("Role name cannot be null")
                .NotEmpty().WithMessage("Role name is required");

            RuleFor(x => x.PermissionNames)
                .NotNull().WithMessage("Permission names cannot be null")
                .NotEmpty().WithMessage("Permission names are required")
                .Matches(@"^[a-zA-Z]+(,\s*[a-zA-Z]+)*$")
                .WithMessage("Permission names must be a comma-separated list of words (e.g. 'create, read')");

            RuleFor(x => x.MenuName)
                .NotNull().WithMessage("Menu name cannot be null")
                .NotEmpty().WithMessage("Menu name is required");
        }
    }
}
