using FluentValidation;
using StudentProj.Application.DTO;

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
                .Matches(@"^(create|read|update|delete)$")
                .WithMessage("Permission must be create, read, update, or delete");
                // Allows letters, colons (:), and hyphens (-) to support formats like "read:student"
                //.Matches("^[a-zA-Z:-]+$")
                //.WithMessage("Permission name can only contain letters, colons (:), and hyphens (-).");
        }
    }
}