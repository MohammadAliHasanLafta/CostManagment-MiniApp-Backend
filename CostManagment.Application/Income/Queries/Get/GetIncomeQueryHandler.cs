using CostManagment.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Income.Queries.Get
{
    public class GetIncomeQueryHandler : IRequestHandler<GetIncomeQuery, int>
    {
        private readonly ICostRepository _costRepository;

        public GetIncomeQueryHandler(ICostRepository costRepository)
        {
            _costRepository = costRepository;
        }

        public async Task<int> Handle(GetIncomeQuery request, CancellationToken cancellationToken)
        {
            var result = await _costRepository.GetSalaryAsync(request.UserId, request.PhoneNumber);
            return result;
        }
    }
}
