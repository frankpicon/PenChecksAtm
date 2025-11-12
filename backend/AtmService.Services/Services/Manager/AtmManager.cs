using AtmService.Services.Commands;
using AtmService.Services.Dtos.v1;
using AtmService.Services.Queries;

namespace AtmService.Services.Manager;

public class AtmManager : IAtmManager
{
    private readonly IAccountQueryService _queries;
    private readonly IAccountCommandService _commands;

    public AtmManager(IAccountQueryService queries, IAccountCommandService commands)
    {
        _queries = queries;
        _commands = commands;
    }

    public Task<IEnumerable<AccountSummaryDto>> GetAccountsAsync() => _queries.GetAccountsAsync();

    public Task<AccountSummaryDto?> GetAccountAsync(string id) => _queries.GetAccountAsync(id);

    public Task<AccountSummaryDto> DepositAsync(string id, decimal amount) =>
        _commands.DepositAsync(id, amount);

    public Task<AccountSummaryDto> WithdrawAsync(string id, decimal amount) =>
        _commands.WithdrawAsync(id, amount);

    public Task<(AccountSummaryDto From, AccountSummaryDto To)> TransferAsync(TransferRequest request) =>
        _commands.TransferAsync(request);
}
