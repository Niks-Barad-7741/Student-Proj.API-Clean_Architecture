using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name cannot be null")
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long")
                .MaximumLength(30).WithMessage("Name cannot exceed 30 characters")
                .Matches(@"^[A-Za-z]+(?: [A-Za-z]+)*$")
                .WithMessage("Name can contain only letters and single spaces")
                .Matches(@"^\S(.*\S)?$")
                .WithMessage("Name cannot start or end with whitespace");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is Required")
                .EmailAddress()
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Please enter a valid email address")
                .NotNull()
                .WithMessage("Email cannot be null");
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required")
                .NotNull()
                .WithMessage("Address cannot be null")
                .MaximumLength(200);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone number is required")
                .NotNull()
                .WithMessage("Phone number cannot be null")
                .Matches(@"^\d{10}$")
                .MaximumLength(10);


            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is Required")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long")
                .MaximumLength(20);

        }

    }
}
