using AtmService.Services.Dtos.v1;

namespace AtmService.Services.Queries;

public interface IAccountQueryService
{
    Task<IEnumerable<AccountSummaryDto>> GetAccountsAsync();
    Task<AccountSummaryDto?> GetAccountAsync(string id);
}
