using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class EnrollmentValidator : AbstractValidator<EnrollStudentDTO>
    {
        public EnrollmentValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0)
                .WithMessage("StudentId is required and must be valid.");

            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("CourseId is required and must be valid.");
        }
    }

    public class UpdateGradeValidator : AbstractValidator<UpdateGradeDTO>
    {
        public UpdateGradeValidator()
        {
            RuleFor(x => x.Grade)
                .NotEmpty()
                .WithMessage("Grade is required.")
                .InclusiveBetween(0, 100)
                .WithMessage("Grade must be between 0 and 100.");
        }
    }
}
