using CostManagment.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Application.Profile.Commands.Create;

public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, long>
{
    private readonly IProfileRepository _profileRepository;

    public CreateProfileCommandHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<long> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profileId = await _profileRepository.Create(request.UserName, request.Email, request.UserId, request.PhoneNumber);

        return profileId;
    }
}
