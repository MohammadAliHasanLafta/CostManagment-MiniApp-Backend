using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Profile.Commands.Delete;

public class DeleteProfileCommand : IRequest<bool>
{
    public long Id { get; set; }
}
