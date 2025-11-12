using AtmService.Domain.Models;

namespace AtmService.Services.Dtos.v1;

public class AccountSummaryDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Balance { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; } = Enumerable.Empty<Transaction>();
}
