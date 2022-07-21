using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Account
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            
            RuleFor(m => m.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Incorrect format of email");

            RuleFor(m => m.Password)
                .MinimumLength(6)
                .WithMessage("Password is too short");

            RuleFor(x => x.PasswordConfirmation)
                .Equal(x => x.Password)
                .WithMessage("Password and confirm password are not the same");

            RuleFor(x => x.PhoneNumber)
                .Matches("^[0-9]*$")
                .WithMessage("Only digits allowed");
        }
    }
}

            
