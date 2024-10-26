using CostManagment.Domain.Interfaces;
using MediatR;

namespace CostManagment.Application.Profile.Commands.Update;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
{
    private readonly IProfileRepository _profileRepository;

    public UpdateProfileCommandHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var result = await _profileRepository.Update(request.UserName, request.Email, request.Id);

        return result;
    }
}
