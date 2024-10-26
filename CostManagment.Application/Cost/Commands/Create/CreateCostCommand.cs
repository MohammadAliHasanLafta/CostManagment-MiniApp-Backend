using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Cost.Commands.Create;

public class CreateCostCommand : IRequest<long>
{
    public string? Titel { get; set; }
    public int Amount { get; set; }
    public long UserId { get; set; }
    public string? PhoneNumber { get; set; }
}
