using CostManagment.Domain.Entities;
using CostManagment.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Profile.Queries.Get;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfile>
{
    private readonly IProfileRepository _profileRepository;

    public GetProfileQueryHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<UserProfile> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.Get(request.UserId, request.PhoneNumber);
        return profile;
    }
}
