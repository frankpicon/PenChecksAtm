using AtmService.Services.Dtos.v1;
using AtmService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using AtmService.Domain.Models;

namespace AtmService.Services.Commands;

public class AccountCommandService : IAccountCommandService
{
    private readonly AtmDbContext _db;

    public AccountCommandService(AtmDbContext db)
    {
        _db = db;
    }

    public async Task<AccountSummaryDto> DepositAsync(string id, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.", nameof(amount));

        var account = await _db.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new InvalidOperationException("Account not found.");

        account.Balance += amount;

        var transaction = new Transaction
        {
            AccountId = account.Id,
            Timestamp = DateTime.UtcNow,
            Type = TransactionType.Deposit,
            Amount = amount,
            BalanceAfter = account.Balance,
            Description = "Deposit"
        };

        _db.Transactions.Add(transaction);

        account.Transactions.Add(transaction);

        await _db.SaveChangesAsync();

        return MapToDto(account);
    }

    public async Task<AccountSummaryDto> WithdrawAsync(string id, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive.", nameof(amount));

        var account = await _db.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new InvalidOperationException("Account not found.");

        if (account.Balance < amount)
            throw new InvalidOperationException("Insufficient funds.");

        account.Balance -= amount;

        var transaction = new Transaction
        {
            AccountId = account.Id,
            Timestamp = DateTime.UtcNow,
            Type = TransactionType.Withdrawal,
            Amount = amount,
            BalanceAfter = account.Balance,
            Description = "Withdrawal"
        };

        _db.Transactions.Add(transaction);

        account.Transactions.Add(transaction);

        await _db.SaveChangesAsync();

        return MapToDto(account);
    }

    public async Task<(AccountSummaryDto From, AccountSummaryDto To)> TransferAsync(TransferRequest request)
    {
        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be positive.", nameof(request.Amount));

        if (request.FromAccountId == request.ToAccountId)
            throw new ArgumentException("Cannot transfer to the same account.");

        var from = await _db.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == request.FromAccountId)
            ?? throw new InvalidOperationException("Source account not found.");

        var to = await _db.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == request.ToAccountId)
            ?? throw new InvalidOperationException("Destination account not found.");

        if (from.Balance < request.Amount)
            throw new InvalidOperationException("Insufficient funds in source account.");

        from.Balance -= request.Amount;
        to.Balance += request.Amount;

        var fromTransaction = new Transaction
        {
            AccountId = from.Id,
            Timestamp = DateTime.UtcNow,
            Type = TransactionType.TransferOut,
            Amount = request.Amount,
            BalanceAfter = from.Balance,
            Description = $"Transfer to {to.Name}"
        };

        _db.Transactions.Add(fromTransaction);

        from.Transactions.Add(fromTransaction);

        //from.Transactions.Add(new Transaction
        //{
        //    AccountId = from.Id,
        //    Type = TransactionType.TransferOut,
        //    Amount = request.Amount,
        //    BalanceAfter = from.Balance,
        //    Description = $"Transfer to {to.Name}"
        //});
        var toTransaction = new Transaction
        {
            AccountId = to.Id,
            Timestamp = DateTime.UtcNow,
            Type = TransactionType.TransferIn,
            Amount = request.Amount,
            BalanceAfter = to.Balance,
            Description = $"Transfer from {from.Name}"
        };
        _db.Transactions.Add(toTransaction);

        to.Transactions.Add(toTransaction);
        //to.Transactions.Add(new Transaction
        //{
        //    AccountId = to.Id,
        //    Type = TransactionType.TransferIn,
        //    Amount = request.Amount,
        //    BalanceAfter = to.Balance,
        //    Description = $"Transfer from {from.Name}"
        //});

        await _db.SaveChangesAsync();

        return (MapToDto(from), MapToDto(to));
    }

    private static AccountSummaryDto MapToDto(Account a) =>
        new()
        {
            Id = a.Id,
            Name = a.Name,
            Balance = a.Balance,
            Transactions = a.Transactions
                .OrderByDescending(t => t.Timestamp)
                .ToList()
        };
}
