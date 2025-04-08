using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMFApplication.DTOs.Account;
using TMFApplication.Services.Account;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [Authorize(Policy = "CreateAccount")]
    // POST: api/Account
    // Creates a new account with the provided details
    [HttpPost("/Create")]
    public async Task<ActionResult<AccountDto>> CreateNewAccount(CreateAccountDto dto)
    {
        try
        {
            var account = await _accountService.CreateAccountAsync(dto);
            return CreatedAtAction(nameof(GetAccountByCode),
                new { accountCode = account.AccountCode }, account);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [Authorize(Policy = "UpdateAccount")]
    // PATCH: api/Account/{accountCode}/status
    // Updates the active status of an account
    [HttpPatch("{accountCode}/status")]
    public async Task<ActionResult<AccountDto>> UpdateAccountStatus(
        string accountCode, [FromBody] bool isActive)
    {
        try
        {
            var account = await _accountService.UpdateAccountStatusAsync(accountCode, isActive);
            return Ok(account);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "ViewAccount")]
    // GET: api/Account/{accountCode}
    // Retrieves account details by account code
    [HttpGet("{accountCode}")]
    public async Task<ActionResult<AccountDto>> GetAccountByCode(string accountCode)
    {
        var account = await _accountService.GetAccountByCodeAsync(accountCode);
        if (account == null) return NotFound();
        return Ok(account);
    }

    [Authorize(Policy = "ViewAccount")]
    // GET: api/Account
    // Retrieves all accounts in the system
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccounts()
    {
        var accounts = await _accountService.GetAllAccountsAsync();
        return Ok(accounts);
    }

    [Authorize(Policy = "ViewAccount")]
    // PATCH: api/Account/types/{accountTypeCode}/interest-rate
    // Updates the interest rate for a specific account type
    [HttpPatch("types/{accountTypeCode}/interest-rate")]
    public async Task<ActionResult<AccountTypeDto>> UpdateAccountTypeInterestRate(
        string accountTypeCode, [FromBody] decimal interestRate)
    {
        try
        {
            var type = await _accountService.UpdateAccountTypeInterestRateAsync(
                accountTypeCode, interestRate);
            return Ok(type);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [Authorize(Policy = "AccountMetaData")]
    // GET: api/Account/types
    // Retrieves all available account types
    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<AccountTypeDto>>> GetAllAccountTypes()
    {
        var types = await _accountService.GetAllAccountTypesAsync();
        return Ok(types);
    }
    [Authorize(Policy = "AccountMetaData")]
    // GET: api/Account/entity-types
    // Retrieves all account entity types 
    [HttpGet("entity-types")]
    public async Task<ActionResult<IEnumerable<AccountEntityTypeDto>>> GetAllAccountEntityTypes()
    {
        var types = await _accountService.GetAllAccountEntityTypesAsync();
        return Ok(types);
    }
    [Authorize(Policy = "AccountMetaDataAdmin")]
    [HttpPost("types")]
    public async Task<ActionResult<AccountTypeDto>> CreateAccountType(CreateAccountTypeDto dto)
    {
        try
        {
            var type = await _accountService.CreateAccountTypeAsync(dto);
            return CreatedAtAction(nameof(GetAllAccountTypes), type);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AccountMetaDataAdmin")]
    [HttpPost("entity-types")]
    public async Task<ActionResult<AccountEntityTypeDto>> CreateAccountEntityType(CreateAccountEntityTypeDto dto)
    {
        try
        {
            var type = await _accountService.CreateAccountEntityTypeAsync(dto);
            return CreatedAtAction(nameof(GetAllAccountEntityTypes), type);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AccountMetaDataAdmin")]
    [HttpPut("types/{accountTypeCode}")]
    public async Task<ActionResult<AccountTypeDto>> UpdateAccountType(
        string accountTypeCode, UpdateAccountTypeDto dto)
    {
        try
        {
            var type = await _accountService.UpdateAccountTypeAsync(accountTypeCode, dto);
            return Ok(type);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}