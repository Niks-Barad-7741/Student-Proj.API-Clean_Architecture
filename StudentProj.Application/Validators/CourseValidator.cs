using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class CourseValidator : AbstractValidator<CreateCourseDTO>
    {
        public CourseValidator()
        {
            RuleFor(x => x.CourseName)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Course Name is required.")
                .MinimumLength(2)
                .WithMessage("Course Name must be at least 2 characters.")
                .MaximumLength(50)
                .WithMessage("Course Name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s\-]+$")
                .WithMessage("Course Name can only contain letters, numbers, spaces, hyphens, and underscores.")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Course Name cannot be the default word 'string'.");

            RuleFor(x => x.Description)
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Description is required.")
                .MinimumLength(2)
                .WithMessage("Description must be at least 2 characters.")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Description cannot be the default word 'string'.");
        }
    }

    public class UpdateCourseValidator : AbstractValidator<UpdateCourseDTO>
    {
        public UpdateCourseValidator()
        {
            RuleFor(x => x.CourseName)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Course Name is required.")
                .MinimumLength(2)
                .WithMessage("Course Name must be at least 2 characters.")
                .MaximumLength(50)
                .WithMessage("Course Name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s\-]+$")
                .WithMessage("Course Name can only contain letters, numbers, spaces, hyphens, and underscores.")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Course Name cannot be the default word 'string'.");

            RuleFor(x => x.Description)
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Description is required.")
                .MinimumLength(2)
                .WithMessage("Description must be at least 2 characters.")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Description cannot be the default word 'string'.");
        }
    }
}

