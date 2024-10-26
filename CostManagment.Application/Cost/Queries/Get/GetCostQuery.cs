using CostManagment.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Cost.Queries.Get;

public class GetCostQuery : IRequest<IEnumerable<Expense>>
{
    public long UserId { get; set; }
    public string? PhoneNumber { get; set; }
}
