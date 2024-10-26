using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Income.Queries.Get;

public class GetIncomeQuery : IRequest<int>
{
    public long UserId { get; set; }
    public string? PhoneNumber { get; set; }
}
