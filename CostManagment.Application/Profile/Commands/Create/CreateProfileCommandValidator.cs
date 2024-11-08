﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Profile.Commands.Create;

public class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
    }
}
