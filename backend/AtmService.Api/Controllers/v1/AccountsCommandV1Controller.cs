using AtmService.Services.Dtos.v1;
using AtmService.Services.Manager;
using Microsoft.AspNetCore.Mvc;

namespace AtmService.Api.Controllers.v1;

[ApiController]
[Route("api/v1/accounts")]
public class AccountsCommandV1Controller : ControllerBase
{
    private readonly IAtmManager _atm;

    public AccountsCommandV1Controller(IAtmManager atm)
    {
        _atm = atm;
    }

    [HttpPost("{id}/deposit")]
    public async Task<ActionResult<AccountSummaryDto>> Deposit(string id, [FromBody] AmountRequest request)
    {
        try
        {
            var updated = await _atm.DepositAsync(id, request.Amount);
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/withdraw")]
    public async Task<ActionResult<AccountSummaryDto>> Withdraw(string id, [FromBody] AmountRequest request)
    {
        try
        {
            var updated = await _atm.WithdrawAsync(id, request.Amount);
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("transfer")]
    public async Task<ActionResult> Transfer([FromBody] TransferRequest request)
    {
        try
        {
            var (from, to) = await _atm.TransferAsync(request);
            return Ok(new { from, to });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
