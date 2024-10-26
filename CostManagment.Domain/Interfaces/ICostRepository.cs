
using CostManagment.Domain.Entities;

namespace CostManagment.Domain.Interfaces;

public interface ICostRepository
{
    Task<int> GetSalaryAsync(long id, string phoneNumber);
    Task<long> CreateIncomeAsync(Wage model);
    Task<long> CreateAsync(Expense model);
    Task<bool> UpdateAsync(long id, string title, int amount);
    Task<bool> DeleteAsync(long id);
    Task<Expense> GetByIdAsync(long id);
    Task<IEnumerable<Expense>> GetAllAsync(long id, string phoneNumber);
}
