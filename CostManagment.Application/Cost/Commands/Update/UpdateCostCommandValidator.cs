using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Cost.Commands.Update;

public class UpdateCostCommandValidator : AbstractValidator<UpdateCostCommand>
{
    public UpdateCostCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount is required");
    }
}
