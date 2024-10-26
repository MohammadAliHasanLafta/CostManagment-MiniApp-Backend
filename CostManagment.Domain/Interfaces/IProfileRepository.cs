using CostManagment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Domain.Interfaces;

public interface IProfileRepository
{
    Task<bool> ProfileIsExist(long userId, string phoneNumber);
    Task<long> Create(string userName, string email, long userId, string phoneNumber);
    Task<UserProfile> Get(long userId, string phoneNumber);
    Task<bool> Update(string userName, string email, long Id);
    Task<bool> Delete(long Id);
    Task<UserProfile> GetById(long Id);
}
