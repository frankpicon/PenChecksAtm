using AtmService.Services.Dtos.v1;
using AtmService.Services.Manager;
using Microsoft.AspNetCore.Mvc;

namespace AtmService.Api.Controllers.v1;

[ApiController]
[Route("api/v1/accounts")]
public class AccountsQueryV1Controller : ControllerBase
{
    private readonly IAtmManager _atm;

    public AccountsQueryV1Controller(IAtmManager atm)
    {
        _atm = atm;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountSummaryDto>>> GetAll()
    {
        var accounts = await _atm.GetAccountsAsync();
        return Ok(accounts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountSummaryDto>> GetById(string id)
    {
        var account = await _atm.GetAccountAsync(id);
        if (account == null) return NotFound();
        return Ok(account);
    }
}
