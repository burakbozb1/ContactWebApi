using ContactWeb.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContactWeb.Service.Validation
{
    public class AddContactDtoValidator: AbstractValidator<AddContactDto>
    {
        public AddContactDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(x => x.Surname)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.PhoneNumber)
              .NotEmpty()
              .NotNull().WithMessage("{PropertyName} is required.")
              .MinimumLength(10).WithMessage("{PropertyName} must not be less than 10 characters.")
              .MaximumLength(20).WithMessage("{PropertyName} must not exceed 50 characters.")
              .Matches(new Regex(@"^[0-9]+$")).WithMessage("PhoneNumber not valid");
        }
    }
}
