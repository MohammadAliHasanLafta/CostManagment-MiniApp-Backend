using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Income.Commands.Create;

public class CreateIncomeCommand : IRequest<long>
{
    public int Salary { get; set; }
    public long UserId { get; set; }
    public string? PhoneNumber { get; set; }
}
