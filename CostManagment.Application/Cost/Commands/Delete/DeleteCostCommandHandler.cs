using CostManagment.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Cost.Commands.Delete;

public class DeleteCostCommandHandler : IRequestHandler<DeleteCostCommand, bool>
{
    private readonly ICostRepository _costRepository;

    public DeleteCostCommandHandler(ICostRepository costRepository)
    {
        _costRepository = costRepository;
    }

    public async Task<bool> Handle(DeleteCostCommand request, CancellationToken cancellationToken)
    {
        var result = await _costRepository.DeleteAsync(request.Id);

        return result;
    }
}
