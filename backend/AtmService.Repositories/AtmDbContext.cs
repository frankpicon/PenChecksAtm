using AtmService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AtmService.Infrastructure;

public class AtmDbContext : DbContext
{
    public AtmDbContext(DbContextOptions<AtmDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id)
                .ValueGeneratedNever();  // manual control "CHK"/"SVG"
            entity.HasMany(a => a.Transactions)
                  .WithOne(t => t.Account)
                  .HasForeignKey(t => t.AccountId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id)
                .ValueGeneratedOnAdd();
        });

        // default demo accounts
        modelBuilder.Entity<Account>().HasData(
            new Account
            {
                Id = "CHK",
                Name = "Checking",
                Balance = 1000m
            },
            new Account
            {
                Id = "SVG",
                Name = "Savings",
                Balance = 500m
            }
        );
    }

}
