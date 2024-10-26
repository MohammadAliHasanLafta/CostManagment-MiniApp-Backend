﻿using CostManagment.Domain.Entities;
using CostManagment.Domain.Interfaces;
using CostManagment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
        await _context.Profiles.AddAsync(newProfile);
        await _context.SaveChangesAsync();

        return newProfile.Id;
    }

    public async Task<UserProfile> Get(long userId, string phoneNumber)
    {
        var profile = new UserProfile();
        var profileIsExist = await ProfileIsExist(userId, phoneNumber);
        Console.WriteLine(profileIsExist);

        if (!profileIsExist)
        {
            var id = await Create("نام کاربر", "example@gmail.com", userId, phoneNumber);
            profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Id == id); 
        }
        else
        {
            if (userId != 0)
            {
                profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId); 
            }
            else
            {
                profile = await _context.Profiles.FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber); 
            }
        }

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
        if (userId > 0)
        {
            return await _context.Profiles.AnyAsync(p => p.UserId == userId);
        }

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            return await _context.Profiles.AnyAsync(p => p.PhoneNumber == phoneNumber);
        }

        return false;
    }

    public async Task<UserProfile> GetById(long Id)
    {
        var profile = _context.Profiles.FirstOrDefault(p => p.Id == Id);

        return profile;
    }

    public async void DeleteCostsOfUser(long? userId, string phoneNumber)
    {
        var costs = new List<Expense>();
        if (userId > 0 || phoneNumber != null)
        {
            costs = _context.Costs
            .Where(cost => cost.UserPhoneNumber == phoneNumber && cost.IsDeleted == false)
            .ToList();
            var incomew = _context.Incomes.FirstOrDefault(i => i.UserPhoneNumber ==  phoneNumber);
            _context.Incomes.Remove(incomew);
        }
        else
        {
            costs = _context.Costs
            .Where(cost => cost.UserId == userId && cost.IsDeleted == false)
            .ToList();
            var incomem = _context.Incomes.FirstOrDefault(i => i.UserId == userId);
            _context.Incomes.Remove(incomem);
        }

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
