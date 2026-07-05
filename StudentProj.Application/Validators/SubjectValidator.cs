using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class SubjectValidator : AbstractValidator<CreateSubjectDTO>
    {
        public SubjectValidator()
        {
            RuleFor(x => x.SubjectName)
                .NotEmpty()
                .WithMessage("Subject Name is required.")
                .MinimumLength(2)
                .WithMessage("Subject Name must be at least 2 characters.")
                .MaximumLength(50)
                .WithMessage("Subject Name cannot exceed 50 characters.")
                .Matches(@"^[A-Za-z0-9\s\-_]+$")
                .WithMessage("Subject Name can only contain letters, numbers, spaces, hyphens, and underscores.")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Subject Name cannot be the default word 'string'.");

            RuleFor(x => x.SubjectCode)
                .GreaterThan(0)
                .WithMessage("Subject Code must be greater than 0.");

            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("CourseId is required and must be valid.");
        }
    }

    public class UpdateSubjectValidator : AbstractValidator<UpdateSubjectDTO>
    {
        public UpdateSubjectValidator()
        {
            RuleFor(x => x.SubjectName)
                .NotEmpty()
                .WithMessage("Subject Name is required.")
                .MinimumLength(2)
                .WithMessage("Subject Name must be at least 2 characters.")
                .MaximumLength(50)
                .WithMessage("Subject Name cannot exceed 50 characters.")
                .Matches(@"^[A-Za-z0-9\s\-_]+$")
                .WithMessage("Subject Name can only contain letters, numbers, spaces, hyphens, and underscores.")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Subject Name cannot be the default word 'string'.");

            RuleFor(x => x.SubjectCode)
                .GreaterThan(0)
                .WithMessage("Subject Code must be greater than 0.");
            
            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("CourseId is required and must be valid.");
        }
    }
}
