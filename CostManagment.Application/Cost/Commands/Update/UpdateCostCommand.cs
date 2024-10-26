using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Cost.Commands.Update;

public class UpdateCostCommand : IRequest<bool>
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public int Amount { get; set; }
}
