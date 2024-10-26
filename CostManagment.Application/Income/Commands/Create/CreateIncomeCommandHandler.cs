using CostManagment.Domain.Entities;
using CostManagment.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Income.Commands.Create;

public class CreateIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, long>
{
    private readonly ICostRepository _costRepository;

    public CreateIncomeCommandHandler(ICostRepository costRepository)
    {
        _costRepository = costRepository;
    }

    public async Task<long> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var result = await _costRepository.CreateIncomeAsync(request.Salary, request.UserId, request.PhoneNumber);

        return result;
    }
}
