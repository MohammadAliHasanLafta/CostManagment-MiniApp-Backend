using CostManagment.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Domain.Entities;

public class Wage : EntityBase
{
    public Wage()
    {
    }

    public Wage(int salary, long? userId = null, string? userPhoneNumber = null)
    {
        if (string.IsNullOrWhiteSpace(userPhoneNumber) && userId == null)
        {
            throw new ArgumentException("Either UserId or PhoneNumber must be provided.");
        }
        Salary = salary;
        UserId = userId;
        UserPhoneNumber = userPhoneNumber;
    }

    public int Salary { get; set; }
    public long? UserId { get; set; }
    public string? UserPhoneNumber { get; set; }
}
