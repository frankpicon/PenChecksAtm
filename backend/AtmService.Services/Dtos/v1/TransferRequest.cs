namespace AtmService.Services.Dtos.v1;

public class TransferRequest
{
    public string FromAccountId { get; set; } = default!;
    public string ToAccountId { get; set; } = default!;
    public decimal Amount { get; set; }
}
