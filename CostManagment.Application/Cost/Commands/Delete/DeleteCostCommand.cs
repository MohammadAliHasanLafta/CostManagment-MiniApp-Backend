using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Cost.Commands.Delete;

public class DeleteCostCommand : IRequest<bool>
{
    public long Id { get; set; }
}
