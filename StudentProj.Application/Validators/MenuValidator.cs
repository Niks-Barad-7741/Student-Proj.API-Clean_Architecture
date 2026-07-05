using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class MenuValidator : AbstractValidator<MenuDTO>
    {
        public MenuValidator()
        {
            RuleFor(x => x.MenuName)
                .NotNull().WithMessage("Menu name cannot be null")
                .NotEmpty().WithMessage("Menu name is required")
                .MinimumLength(2).WithMessage("Menu name must be at least 2 characters")
                .MaximumLength(50).WithMessage("Menu name cannot exceed 50 characters")
                .MinimumLength(3).WithMessage("Menu name must be at least 3 characters long")
                .Matches(@"^[a-zA-Z][a-zA-Z0-9\-]*$")
                .WithMessage("Menu name must start with a letter and can contain alphanumeric characters and hyphens.")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Menu name cannot be the default word 'string'.");

            RuleFor(x => x.MenuRoute)
                .NotNull().WithMessage("Menu route cannot be null")
                .NotEmpty().WithMessage("Menu route is required")
                .MinimumLength(2).WithMessage("Menu route must be at least 2 characters")
                .MaximumLength(100).WithMessage("Menu route cannot exceed 100 characters")
                .Matches(@"^/[a-zA-Z0-9\-/]*$")
                .WithMessage("Menu route must start with '/' and can only contain letters, numbers, hyphens, and slashes")
                .NotEqual("/string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Menu route cannot be the default word '/string'.");
        }
    }
}
