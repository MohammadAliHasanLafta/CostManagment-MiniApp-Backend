using CostManagment.Domain.Entities;
using CostManagment.Domain.Interfaces;
using MediatR;

namespace CostManagment.Application.Cost.Queries.Get;

public class GetCostQueryHandler : IRequestHandler<GetCostQuery, IEnumerable<Expense>>
{
    private readonly ICostRepository _costRepository;

    public GetCostQueryHandler(ICostRepository costRepository)
    {
        _costRepository = costRepository;
    }

    public async Task<IEnumerable<Expense>> Handle(GetCostQuery request, CancellationToken cancellationToken)
    {
        var result = await _costRepository.GetAllAsync(request.UserId, request.PhoneNumber);

        return result;
    }
}
