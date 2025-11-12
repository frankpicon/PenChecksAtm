using AtmService.Services.Dtos.v1;

namespace AtmService.Services.Commands;

public interface IAccountCommandService
{
    Task<AccountSummaryDto> DepositAsync(string id, decimal amount);
    Task<AccountSummaryDto> WithdrawAsync(string id, decimal amount);
    Task<(AccountSummaryDto From, AccountSummaryDto To)> TransferAsync(TransferRequest request);
}
