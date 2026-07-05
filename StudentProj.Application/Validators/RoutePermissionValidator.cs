using FluentValidation;
using StudentProj.Application.DTOs;

namespace StudentProj.Application.Validators
{
    public class RoutePermissionValidator : AbstractValidator<RoutePermissionDTO>
    {
        public RoutePermissionValidator()
        {
            RuleFor(x => x.HttpMethod)
                .NotEmpty().WithMessage("HTTP Method is required!")
                .NotNull().WithMessage("HTTP Method cannot be null!")
                .Must(method => new[] { "GET", "POST", "PUT", "PATCH", "DELETE" }.Contains(method.ToUpperInvariant()))
                .WithMessage("HTTP Method must be one of: GET, POST, PUT, PATCH, DELETE.");

            RuleFor(x => x.PathPattern)
                .NotEmpty().WithMessage("Path Pattern is required!")
                .NotNull().WithMessage("Path Pattern cannot be null!")
                .MaximumLength(250).WithMessage("Path Pattern cannot exceed 250 characters!")
                .Matches(@"^\/api\/.*$").WithMessage("Path Pattern must start with '/api/'")
                .NotEqual("/api/", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Path Pattern must specify a valid route, not just '/api/'.")
                .NotEqual("/api/string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Path Pattern cannot be the default word '/api/string'.");

            RuleFor(x => x.RequiredMenuName)
                .NotEmpty().WithMessage("Required Menu Name is required!")
                .NotNull().WithMessage("Required Menu Name cannot be null!")
                .MaximumLength(50).WithMessage("Required Menu Name cannot exceed 50 characters!")
                                .Matches(@"^\S(.*\S)?$")
                .WithMessage("Cannot start or end with whitespace")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Cannot be the default word 'string'.");

            RuleFor(x => x.RequiredPermissionName)
                .NotEmpty().WithMessage("Required Permission Name is required!")
                .NotNull().WithMessage("Required Permission Name cannot be null!")
                .MaximumLength(50).WithMessage("Required Permission Name cannot exceed 50 characters!")
                .Matches(@"^\S(.*\S)?$")
                .WithMessage("Cannot start or end with whitespace")
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Cannot be the default word 'string'.");
        }
    }
}
