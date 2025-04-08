using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMFApplication.Contracts.Admin.Commands;
using TMFApplication.Contracts.Admin.Queries;
using TMFApplication.DTOs.Admin;
using TMFApplication.Utilities;

namespace TasawabMicroFinanceCitta.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : Controller
    {
        private readonly IMediator _mediator;

        public StaffController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Policy = "ViewStaffs")]
        [HttpGet("get-staff/{staffCode}")]
        public async Task<IActionResult> GetStaffWithUser(string staffCode)
        {
            try
            {
                var query = new GetStaffWithUserQuery { StaffCode = staffCode };
                var result = await _mediator.Send(query);

                if (result == null || !result.Success)
                {
                    return NotFound(new { success = false, message = "Staff not found or has been deleted" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [Authorize(Policy = "ViewStaffs")]
        [HttpGet("get-allstaffs")]
        public async Task<IActionResult> GetStaffs()
        {
            try
            {
                var query = new GetStaffsQuery();
                var result = await _mediator.Send(query);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Policy = "CreateStaff")]
        [HttpPost("create-staff")]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffRequest request)
        {
            try
            {
                var command = new CreateStaffCommand { Request = request };
                var result = await _mediator.Send(command);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [Authorize(Policy = "UpdateStaff")]
        [HttpPut("edit-staff")]
        public async Task<IActionResult> EditStaff([FromBody] EditStaffRequest request)
        {
            try
            {
                var command = new EditStaffCommand { Request = request };
                var result = await _mediator.Send(command);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [Authorize(Policy = "DeactivateStaff")]
        [HttpPost("deactivate-staff")]
        public async Task<IActionResult> DeactivateStaff([FromBody] DeactivateStaffRequest request)
        {
            try
            {
                var command = new DeactivateStaffCommand { Request = request };
                var result = await _mediator.Send(command);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Policy = "ActivateStaff")]
        [HttpPost("activate-staff")]
        public async Task<IActionResult> ActivateStaff([FromBody] ActivateStaffRequest request)
        {
            try
            {
                var command = new ActivateStaffCommand { Request = request };
                var result = await _mediator.Send(command);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [Authorize(Policy = "DeleteStaff")]
        [HttpDelete("delete-staff")]
        public async Task<IActionResult> DeleteStaff([FromBody] DeleteStaffRequest request)
        {
            try
            {
                var command = new DeleteStaffCommand { Request = request };
                var result = await _mediator.Send(command);

                if (result.Success)
                {
                    return Ok(new { success = true, message = "Staff has been deleted" });
                }
                return BadRequest(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }



    }
}