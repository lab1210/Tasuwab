using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TMFApplication.DTOs.Loan;
using TMFDomain.Entities.Loan;
using TMFDomain.Interfaces.Loan;

namespace TasawabMicroFinanceCitta.Controllers.Loan
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "DynamicPermission")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        // Loan Type
        [HttpPost("LoanType")]
        [Authorize(Policy = "DynamicPermission", Roles = "AddLoanType")]

        public async Task<IActionResult> AddLoanType([FromBody] AddLoanTypeRequest request)
        {
            var loanType = new LoanType
            {
                LoanTypeId = Guid.NewGuid().ToString(),
                LoanName = request.LoanName,
                LoanDescription = request.LoanDescription,
                InterestRate = request.InterestRate,
                PenaltyRate = request.PenaltyRate,
                TenureYears = request.TenureYears,
                InstallmentFrequency = request.InstallmentFrequency,
                PerformedBy = User.Identity.Name 
            };

            var result = await _loanService.AddLoanTypeAsync(loanType, User.Identity.Name);
            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new { message = result.message });
        }
        [HttpGet("LoanType")]
        [Authorize(Policy = "DynamicPermission", Roles = "GetAllLoanTypes")]
        public async Task<IActionResult> GetAllLoanTypes()
        {
            var loanTypes = await _loanService.GetAllLoanTypesAsync();
            return Ok(loanTypes);
        }

        // Loan Application
        [HttpPost("Application")]
        [Authorize(Policy = "DynamicPermission", Roles = "CreateLoanApplication")]
        public async Task<IActionResult> CreateLoanApplication([FromBody] CreateLoanApplicationRequest request)
        {
            var loanApplication = new LoanApplication
            {
                LoanApplicationId = Guid.NewGuid().ToString(),
                LoanApprovalId = Guid.NewGuid().ToString(),
                AccountCode = request.AccountCode,
                LoanTypeId = request.LoanTypeId,
                PrincipalAmount = request.PrincipalAmount,
                ApprovalStatus = 0, // Pending
                DocumentLinks = string.Join("|", request.DocumentLinks),
                PerformedBy = User.Identity.Name
            };

            var result = await _loanService.CreateLoanApplicationAsync(loanApplication, User.Identity.Name);
            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new { message = result.message });
        }

        [HttpPost("Application/Approve")]
        [Authorize(Policy = "DynamicPermission", Roles = "ApproveLoanApplication")]
        public async Task<IActionResult> ApproveLoanApplication([FromBody] ApproveLoanApplicationRequest request)
        {
            var result = await _loanService.ApproveLoanApplicationAsync(request.LoanApplicationId, request.RoleId, User.Identity.Name);
            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new { message = result.message });
        }

        [HttpGet("Application/{loanApplicationId}/Tracking")]
        [Authorize(Policy = "DynamicPermission", Roles = "GetApprovalTracking")]
        public async Task<IActionResult> GetApprovalTracking(string loanApplicationId)
        {
            var tracking = await _loanService.GetApprovalTrackingAsync(loanApplicationId);
            return Ok(tracking);
        }

        [HttpPut("LoanType/{loanTypeId}")]
        [Authorize(Policy = "DynamicPermission", Roles = "UpdateLoanType")]
        public async Task<IActionResult> UpdateLoanType(string loanTypeId, [FromBody] UpdateLoanTypeRequest request)
        {
            var loanType = new LoanType
            {
                LoanTypeId = loanTypeId,
                LoanName = request.LoanName,
                LoanDescription = request.LoanDescription,
                InterestRate = request.InterestRate,
                PenaltyRate = request.PenaltyRate,
                TenureYears = request.TenureYears,
                InstallmentFrequency = request.InstallmentFrequency,
                PerformedBy = User.Identity.Name // Set the PerformedBy property
            };

            var result = await _loanService.UpdateLoanTypeAsync(loanType, User.Identity.Name);
            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new { message = result.message });
        }
    }
}