using CostManagment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Wage> Incomes { get; set; }
    public DbSet<Expense> Costs { get; set; }
    public DbSet<MiniAppUser> MiniAppUsers { get; set; }
    public DbSet<WebAppUser> WebAppUsers { get; set; }
    public DbSet<UserProfile> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Amount).IsRequired();
            entity.Property(e => e.UserId).IsRequired(false);
            entity.Property(e => e.UserPhoneNumber).IsRequired(false);

            entity.HasCheckConstraint("CK_UserIdOrPhoneNumber",
                "[UserId] IS NOT NULL OR [UserPhoneNumber] IS NOT NULL");
        });

        modelBuilder.Entity<Wage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Salary).IsRequired();
            entity.Property(e => e.UserId).IsRequired(false);
            entity.Property(e => e.UserPhoneNumber).IsRequired(false);

            entity.HasCheckConstraint("CK_UserIdOrPhoneNumber",
                "[UserId] IS NOT NULL OR [UserPhoneNumber] IS NOT NULL");
        });
    }
}
