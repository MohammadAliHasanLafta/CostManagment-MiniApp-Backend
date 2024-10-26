
using CostManagment.Domain.Entities;

namespace CostManagment.Domain.Interfaces;

public interface ITokenService
{
    string CreateToken(MiniAppUser? miniAppUser, WebAppUser? webAppUser);
}
