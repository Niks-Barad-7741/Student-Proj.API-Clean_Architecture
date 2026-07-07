using FluentValidation;
using StudentProj.Application.DTOs;
using System;

namespace StudentProj.Application.Validators
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDTO>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Name)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s\-]+$").WithMessage("Name can only contain letters, numbers, spaces, hyphens, and underscores.")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Name cannot be the default word 'string'.");

            RuleFor(x => x.Description)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(2).WithMessage("Description must be at least 2 characters.")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Description cannot be the default word 'string'.");
        }
    }

    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentDTO>
    {
        public UpdateDepartmentValidator()
        {
            RuleFor(x => x.Name)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s\-]+$").WithMessage("Name can only contain letters, numbers, spaces, hyphens, and underscores.")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Name cannot be the default word 'string'.");

            RuleFor(x => x.Description)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(2).WithMessage("Description must be at least 2 characters.")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Description cannot be the default word 'string'.");
        }
    }
}
