using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class PermissionValidator : AbstractValidator<PermissionDTO>
    {
        public PermissionValidator()
        {
            RuleFor(x => x.PermissionName)
                .NotEmpty()
                .WithMessage("Permission name is required")
                .NotNull()
                .WithMessage("Permission name cannot be null")
                .Matches(@"^(create|read|update|delete)(-[a-zA-Z]+)?$")
                .WithMessage("Permission must be create, read, update, or delete (with optional suffix like -only)");
        }
    }

    public class CreatePermissionValidator : AbstractValidator<CreatePermissionDTO>
    {
        public CreatePermissionValidator()
        {
            RuleFor(x => x.PermissionName)
                .NotEmpty()
                .WithMessage("Permission name is required")
                .NotNull()
                .WithMessage("Permission name cannot be null")
                .Matches(@"^(create|read|update|delete)(-[a-zA-Z]+)?$")
                .WithMessage("Permission must be create, read, update, or delete (with optional suffix like -only)");
        }
    }
}