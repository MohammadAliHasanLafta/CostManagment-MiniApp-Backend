using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Cost.Commands.Create;

public class CreateCostCommandValidator : AbstractValidator<CreateCostCommand>
{
    public CreateCostCommandValidator()
    {
        RuleFor(x => x.Titel).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount is required");
    }
}
