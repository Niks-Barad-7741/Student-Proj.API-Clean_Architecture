using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class StudentValidator : AbstractValidator<StudentDTO>
    {
        public StudentValidator()
        {
            RuleFor(x => x.Name)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotNull().WithMessage("Name cannot be null")
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long")
                .MaximumLength(30).WithMessage("Name cannot exceed 30 characters")
                .Matches(@"^[A-Za-z]+(?: [A-Za-z]+)*$")
                .WithMessage("Name can contain only letters and single spaces")
                .Matches(@"^\S(.*\S)?$")
                .WithMessage("Name cannot start or end with whitespace");

            RuleFor(x => x.Email)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Email is Required")
                .EmailAddress()
                .WithMessage("Invalid Email Address")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Email must be a valid Gmail address");

            RuleFor(x => x.Address)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Address is required")
                .NotNull()
                .WithMessage("Address cannot be null")
                .MaximumLength(200)
                .WithMessage("Address Cannot Exceed 200 Characters");

            RuleFor(x => x.Phone)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Phone number is required")
                .NotNull()
                .WithMessage("Phone number cannot be null")
                .Matches(@"^\d{10}$")
                .WithMessage("Phone number must be exactly 10 digits")
                .MaximumLength(10);
        }
    }
}


