using AtmService.Services.Dtos.v1;
using AtmService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AtmService.Services.Queries;

public class AccountQueryService : IAccountQueryService
{
    private readonly AtmDbContext _db;

    public AccountQueryService(AtmDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AccountSummaryDto>> GetAccountsAsync()
    {
        var accounts = await _db.Accounts
            .AsNoTracking()
            .Include(a => a.Transactions)
            .ToListAsync();

        return accounts.Select(a => new AccountSummaryDto
        {
            Id = a.Id,
            Name = a.Name,
            Balance = a.Balance,
            Transactions = a.Transactions
                .OrderByDescending(t => t.Timestamp)
                .ToList()
        });
    }

    public async Task<AccountSummaryDto?> GetAccountAsync(string id)
    {
        var a = await _db.Accounts
            .AsNoTracking()
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (a == null) return null;

        return new AccountSummaryDto
        {
            Id = a.Id,
            Name = a.Name,
            Balance = a.Balance,
            Transactions = a.Transactions
                .OrderByDescending(t => t.Timestamp)
                .ToList()
        };
    }
}
