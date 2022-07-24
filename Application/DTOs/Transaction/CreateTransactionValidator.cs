using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Transaction
{
    public class CreateTransactionValidator : AbstractValidator<CreateTransactionRequest>
    {
        public CreateTransactionValidator()
        {
            RuleFor(i => i.BudgetId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Transaction can't be specified out of budget");

            RuleFor(t => t.Type)
                .NotEmpty()
                .NotNull()
                .MaximumLength(20)
                .WithMessage("Type of transaction is required");

            RuleFor(c => c.Category)
                .NotEmpty()
                .NotNull()
                .MaximumLength(20)
                .WithMessage("Category must be specified");

            RuleFor(v => v.Value)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Value can't be lower than 0");

            RuleFor(d => d.TransactionDate)
                .NotNull()
                .NotEmpty()
                .WithMessage("Transaction date is required");
        }
    }
}
