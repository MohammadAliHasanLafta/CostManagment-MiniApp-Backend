using CostManagment.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Cost.Commands.Update;

public class UpdateCostCommandHandler : IRequestHandler<UpdateCostCommand, bool>
{
    private readonly ICostRepository _costRepository;

    public UpdateCostCommandHandler(ICostRepository costRepository)
    {
        _costRepository = costRepository;
    }

    public async Task<bool> Handle(UpdateCostCommand request, CancellationToken cancellationToken)
    {
        var result = await _costRepository.UpdateAsync(request.Id, request.Title, request.Amount);

        return result;
    }
}
