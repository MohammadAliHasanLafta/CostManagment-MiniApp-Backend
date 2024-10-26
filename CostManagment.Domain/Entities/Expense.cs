using CostManagment.Domain.Common;

namespace CostManagment.Domain.Entities;

public class Expense : EntityBase
{
    public Expense(string title, int amount, long? userId = null, string? userPhoneNumber = null)
    {
        if (string.IsNullOrWhiteSpace(userPhoneNumber) && userId == null)
        {
            throw new ArgumentException("Either UserId or PhoneNumber must be provided.");
        }

        Title = title;
        Amount = amount;
        UserId = userId;
        UserPhoneNumber = userPhoneNumber;
    }

    public string Title { get; set; }
    public int Amount { get; set; }
    public long? UserId { get; set; }
    public string? UserPhoneNumber { get; set; }
}
