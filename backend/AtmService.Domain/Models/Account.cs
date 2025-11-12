namespace AtmService.Domain.Models;

public class Account
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Balance { get; set; }
    public List<Transaction> Transactions { get; set; } = new();
}
