using CostManagment.Domain.Entities;
using CostManagment.Domain.Interfaces;
using CostManagment.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Infrastructure.Repository;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _context;

    public ProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<long> Create(string userName, string email, long userId, string phoneNumber)
    {
        var profileIsExist = await ProfileIsExist(userId, phoneNumber);
        if (profileIsExist)
        {
            return 0000;
        }
        var newProfile = new UserProfile(userName, email, userId, phoneNumber);
        _context.Profiles.Add(newProfile);
        _context.SaveChanges();

        return newProfile.Id;
    }

    public async Task<UserProfile> Get(long userId, string phoneNumber)
    {
        var profile = new UserProfile();
        var profileIsExist = await ProfileIsExist(userId, phoneNumber);
        if (!profileIsExist)
        {
            var id = await Create("نام کاربر", "example@gmail.com", userId, phoneNumber);
            profile = _context.Profiles.FirstOrDefault(p => p.Id == id);
        }
        else
            profile = _context.Profiles.FirstOrDefault(p => p.UserId == userId || p.PhoneNumber == phoneNumber);

        _context.SaveChanges();

        return profile;
    }

    public async Task<bool> Update(string userName, string email, long Id)
    {
        var profile = await GetById(Id);

        if (profile == null) return false;

        profile.Email = email;
        profile.UserName = userName;
        profile.UpdatedAt = DateTime.Now;

        _context.SaveChanges();

        return true;
    }

    public async Task<bool> Delete(long Id)
    {
        var profile = await GetById(Id);

        if (profile == null) return false;

        _context.Profiles.Remove(profile);
        _context.SaveChanges();

        DeleteCostsOfUser(profile.UserId, profile.PhoneNumber);
        Console.WriteLine(Id);
        return true;

    }

    public async Task<bool> ProfileIsExist(long userId, string phoneNumber)
    {
        var profileisExist = _context.Profiles.Any(p => p.UserId == userId || p.PhoneNumber == phoneNumber);
        return profileisExist;
    }

    public async Task<UserProfile> GetById(long Id)
    {
        var profile = _context.Profiles.FirstOrDefault(p => p.Id == Id);

        return profile;
    }

    public async void DeleteCostsOfUser(long? userId, string phoneNumber)
    {
        var costs = new List<Expense>();
        if (phoneNumber != null)
            costs = _context.Costs
            .Where(cost => cost.UserPhoneNumber == phoneNumber && cost.IsDeleted == false)
            .ToList();


        costs = _context.Costs
            .Where(cost => cost.UserId == userId && cost.IsDeleted == false)
            .ToList();

        if (costs.Any())
        {

            foreach (var cost in costs)
            {
                cost.IsDeleted = true;
            }
            Console.WriteLine(phoneNumber);
            _context.SaveChanges();
        }
    }
}
