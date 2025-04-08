using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMFApplication.DTOs.Transaction;
using TMFApplication.Services.Transaction;

namespace TasawabMicroFinanceCitta.Controllers.Client
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize(Policy = "PostTransaction")]
        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateTransaction(CreateTransactionDto dto)
        {
            try
            {
                var performedBy = User.Identity.Name; // Get current user
                var transaction = await _transactionService.CreateTransactionAsync(dto, performedBy);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [Authorize(Policy = "ViewTransation")]
        [HttpGet("account/{accountCode}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAccountTransactions(string accountCode)
        {
            var transactions = await _transactionService.GetAccountTransactionsAsync(accountCode);
            return Ok(transactions);
        }

        [Authorize(Policy = "ViewTransation")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransactionById(int id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    return NotFound();
                }
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "ViewTransactionCharge")]
        [HttpGet("charges")]
        public async Task<ActionResult<IEnumerable<TransactionChargeDto>>> GetAllTransactionCharges()
        {
            var charges = await _transactionService.GetAllTransactionChargesAsync();
            return Ok(charges);
        }
       
        [Authorize(Policy = "CreateTransactionCharge")]
        [HttpPost("charges")]
        [ProducesResponseType(typeof(TransactionChargeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionChargeDto>> CreateTransactionCharge(
    [FromBody] CreateTransactionChargeDto dto)
        {
            try
            {
                var charge = await _transactionService.CreateTransactionChargeAsync(dto);
                return CreatedAtAction(nameof(GetTransactionCharge), new { id = charge.Id }, charge);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "UpdateTransactionCharge")]
        [HttpPut("charges/{id}")]
        [ProducesResponseType(typeof(TransactionChargeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionChargeDto>> UpdateTransactionCharge(
            int id, [FromBody] UpdateTransactionChargeDto dto)
        {
            try
            {
                var charge = await _transactionService.UpdateTransactionChargeAsync(id, dto);
                return Ok(charge);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "ViewTransactionCharge")]
        [HttpGet("charges/{id}")]
        [ProducesResponseType(typeof(TransactionChargeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionChargeDto>> GetTransactionCharge(int id)
        {
            var charge = await _transactionService.GetTransactionChargeByIdAsync(id);
            if (charge == null)
            {
                return NotFound();
            }
            return Ok(charge);
        }
    }
}