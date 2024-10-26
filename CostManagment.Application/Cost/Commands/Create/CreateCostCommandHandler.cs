using CostManagment.Domain.Entities;
using CostManagment.Domain.Interfaces;
using MediatR;

namespace CostManagment.Application.Cost.Commands.Create;

public class CreateCostCommandHandler : IRequestHandler<CreateCostCommand, long>
{
    private readonly ICostRepository _costRepository;

    public CreateCostCommandHandler(ICostRepository costRepository)
    {
        _costRepository = costRepository;
    }

    public async Task<long> Handle(CreateCostCommand request, CancellationToken cancellationToken)
    {
        var result = await _costRepository.CreateAsync(new Expense(request.Titel, request.Amount, request.UserId, request.PhoneNumber));

        return result;
    }
}
