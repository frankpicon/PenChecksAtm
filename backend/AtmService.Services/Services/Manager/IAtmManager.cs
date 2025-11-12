using AtmService.Services.Dtos.v1;

namespace AtmService.Services.Manager;

public interface IAtmManager
{
    Task<IEnumerable<AccountSummaryDto>> GetAccountsAsync();
    Task<AccountSummaryDto?> GetAccountAsync(string id);
    Task<AccountSummaryDto> DepositAsync(string id, decimal amount);
    Task<AccountSummaryDto> WithdrawAsync(string id, decimal amount);
    Task<(AccountSummaryDto From, AccountSummaryDto To)> TransferAsync(TransferRequest request);
}
