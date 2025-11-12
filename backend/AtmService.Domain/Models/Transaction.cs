using System.Text.Json.Serialization;

namespace AtmService.Domain.Models;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
    public string? Description { get; set; }

    public string AccountId { get; set; } = default!;
    [JsonIgnore]
    public Account Account { get; set; } = default!;
}
