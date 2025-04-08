using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Staff;
using TMFApplication.DTOs.Admin;
using TMFInfrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace TasawabMicroFinanceCitta.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchRepository _branchRepository;

        public BranchController(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }
        [Authorize(Policy = "AddBranch")]
        [HttpPost("add")]
        public async Task<IActionResult> AddBranch([FromBody] CreateBranchDto branchDto)
        {
            try
            {
                var branch = new Branch
                {
                    name = branchDto.name,
                    description = branchDto.description,
                    email = branchDto.email,
                    phone = branchDto.phone,
                };

                var addedBranch = await _branchRepository.AddAsync(branch);
                return Ok(new { success = true, message = "Branch added successfully.", branch = addedBranch });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [Authorize(Policy = "UpdateBranch")]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditBranch(int id, [FromBody] UpdateBranchDto branchDto)
        {
            try
            {
                var branch = await _branchRepository.GetByIdAsync(id);
                if (branch == null)
                {
                    return NotFound(new { success = false, message = "Branch not found." });
                }

                if (branchDto.description != null)
                {
                    branch.description = branchDto.description;
                }
                if (!string.IsNullOrWhiteSpace(branchDto.email))
                {
                    branch.email = branchDto.email;
                }
                if (!string.IsNullOrWhiteSpace(branchDto.email))
                {
                    branch.phone = branchDto.phone;
                }

                await _branchRepository.UpdateAsync(branch);
                return Ok(new { success = true, message = "Branch updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [Authorize(Policy = "ActivateBranch")]
        [HttpPost("activate/{id}")]
        public async Task<IActionResult> ActivateBranch(int id)
        {
            await _branchRepository.ActivateAsync(id);
            return Ok(new { success = true, message = "Branch activated successfully." });
        }
        [Authorize(Policy = "DeactivateBranch")]
        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateBranch(int id)
        {
            await _branchRepository.DeactivateAsync(id);
            return Ok(new { success = true, message = "Branch deactivated successfully." });
        }
        [Authorize(Policy = "DeleteBranch")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            bool softDeleted = await _branchRepository.DeleteAsync(id);
            if (!softDeleted)
            {
                return NotFound(new { success = false, message = "Branch not found." });
            }

            return Ok(new { success = true, message = "Branch soft deleted successfully." });
        }
        // [Authorize(Policy = "ViewBranch")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBranches()
        {
            var branches = await _branchRepository.GetAllAsync();
            return Ok(branches);
        }
        [Authorize(Policy = "ViewBranch")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var department = await _branchRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound(new { success = false, message = "Position not found." });
            }

            return Ok(department);
        }
    }
}