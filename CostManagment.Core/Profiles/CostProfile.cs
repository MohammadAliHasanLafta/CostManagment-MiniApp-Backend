using AutoMapper;
using CostManagment.Core.Dtos;
using CostManagment.Domain.Entities;

namespace CostManagment.Core.Profiles;

public class CostProfile : Profile
{
    public CostProfile()
    {
        CreateMap<Expense, CostDto>().ReverseMap();
    }
}
