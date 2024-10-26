using CostManagment.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Profile.Queries.Get;

public class GetProfileQuery : IRequest<UserProfile>
{
    public long UserId { get; set; }
    public string? PhoneNumber { get; set; }
}
