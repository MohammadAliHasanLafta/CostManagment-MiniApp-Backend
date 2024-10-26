﻿using CostManagment.Domain.Entities;
using CostManagment.Domain.Interfaces;
using CostManagment.Infrastructure.Data;

namespace CostManagment.Infrastructure.Repository;

public class CostRepository : ICostRepository
{
    private readonly AppDbContext _context;

    public CostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<long> CreateIncomeAsync(Wage model)
    {
        var income = _context.Incomes.FirstOrDefault(i => i.UserId == model.UserId || i.UserPhoneNumber == model.UserPhoneNumber);
        if (income == null)
        {
            var entity = _context.Incomes.Add(model);
            _context.SaveChanges();

            return entity.Entity.Id;
        }

        income.Salary = model.Salary;
        _context.SaveChanges();

        return income.Id;
    }

    public async Task<long> CreateAsync(Expense model)
    {

        var entity = _context.Costs.Add(model);
        _context.SaveChanges();

        return entity.Entity.Id;
    }

    public async Task<bool> UpdateAsync(long id, string title, int amount)
    {
        var cost = await GetByIdAsync(id);

        if (cost == null) return false;

        cost.Title = title;
        cost.Amount = amount;
        cost.UpdatedAt = DateTime.Now;

        _context.SaveChanges();

        return true;
    }

    public async Task<Expense> GetByIdAsync(long id)
    {
        return _context.Costs.FirstOrDefault(t => t.Id == id);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var cost = await GetByIdAsync(id);

        if (cost == null) return false;

        cost.IsDeleted = true;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Expense>> GetAllAsync(long userId, string phoneNumber)
    {
        if (phoneNumber != null)
            return _context.Costs
            .Where(cost => cost.UserPhoneNumber == phoneNumber && cost.IsDeleted == false)
            .ToList();


        return _context.Costs
            .Where(cost => cost.UserId == userId && cost.IsDeleted == false)
            .ToList();
    }

    public async Task<int> GetSalaryAsync(long id, string phoneNumber)
    {
        var income = _context.Incomes.FirstOrDefault(i => i.UserId == id || i.UserPhoneNumber == phoneNumber);
        if (income == null)
            return 0;

        return income.Salary;
    }
}