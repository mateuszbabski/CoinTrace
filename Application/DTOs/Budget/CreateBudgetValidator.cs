using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Budget
{
    public class CreateBudgetValidator : AbstractValidator<CreateBudgetRequest>
    {
        public CreateBudgetValidator()
        {
            RuleFor(n => n.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Name can not be empty");

            RuleFor(n => n.Description)
                .MaximumLength(100);
        }
    }
}
