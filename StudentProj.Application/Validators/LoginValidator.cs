using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .Must(x => x == null || x.Trim() == x).WithMessage("This field cannot contain leading or trailing spaces.")
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Email is Required")
                .EmailAddress()
                .WithMessage("Invalid Email Address")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Please enter a valid email address");
            RuleFor(x => x.Password)
                .Must(x => x == null || !x.Equals("string", StringComparison.OrdinalIgnoreCase)).WithMessage("Default 'string' value is not allowed.")
                .NotEmpty()
                .WithMessage("Password is Required")
                .MinimumLength(6)
                //.WithMessage("Password must be at least 6 characters long")
                .MaximumLength(20).Must(p => p == null || p.Trim() == p).WithMessage("Password cannot start or end with a space.");
        }
    }
}


