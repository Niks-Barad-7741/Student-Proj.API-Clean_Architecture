using FluentValidation;
using StudentProj.Application.DTOs;
using StudentProj.Domain.Common;
using System;

namespace StudentProj.Application.Validators
{
    public class AttendanceValidator : AbstractValidator<RecordAttendanceDTO>
    {
        public AttendanceValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0)
                .WithMessage("StudentId is required and must be valid.");

            RuleFor(x => x.SubjectId)
                .GreaterThan(0)
                .WithMessage("SubjectId is required and must be valid.");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required.")
                .Must(status => new[] { "Present", "Absent", "Late" }.Contains(status, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Status must be one of: Present, Absent, Late.");

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTimeHelper.GetIndianStandardTime().Date.AddDays(1))
                .WithMessage("Attendance date cannot be in the future.");
        }
    }
}
