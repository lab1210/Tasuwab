using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TMFApplication.DTOs.Authentication;
using TMFDomain.Interfaces;

namespace TasawabMicroFinanceCitta.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

        }




        // POST: api/Authentication/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] TMFApplication.DTOs.Authentication.LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request.EmailOrStaffCode, request.Password);
            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new LoginResponse
            {
                Success = result.success,
                Message = result.message,
                Token = result.token,

                // Additional user details
                StaffCode = result.user.staff_code,
                Email = result.user.email,
                Role = result.user.role_code,
                PositionCode = result.user.position_code,
                DepartmentCode = result.user.department_code,
                BranchCode = result.user.branch_code,

                // Staff personal information
                StaffPersonalInformation = new StaffPersonalInformationResponse
                {
                    FirstName = result.staffPersonalInfo.first_name,
                    LastName = result.staffPersonalInfo.last_name,
                    Gender = result.staffPersonalInfo.gender,
                    MartialStatus = result.staffPersonalInfo.martial_status,
                    DateOfBirth = result.staffPersonalInfo.date_of_birth ?? 0,
                    Address = result.staffPersonalInfo.address,
                    Phone = result.staffPersonalInfo.phone,
                    StaffImage = result.staffPersonalInfo.staff_image
                }
            });
        }
        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _authenticationService.RefreshTokenAsync();
            if (!result.success)
            {
                return Unauthorized(new { message = result.message });
            }

            return Ok(new { token = result.token });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authenticationService.LogoutAsync();
            return Ok(new { message = result.message });
        }
        // POST: api/Authentication/ResetPassword
        [HttpPost("ResetPassword")]
public async Task<IActionResult> ResetPassword([FromBody] TMFApplication.DTOs.Authentication.ResetPasswordRequest request)
{
    var result = await _authenticationService.ResetPasswordAsync(request.Email);
    if (!result.success)
        return BadRequest(new { message = result.message });

    return Ok(new ResetPasswordResponse
    {
        Success = result.success,
        Message = result.message
    });
}

        // POST: api/Authentication/InitialPasswordSet
        [HttpPost("InitialPasswordSet")]
        public async Task<IActionResult> InitialPasswordSet([FromBody] InitialPasswordSetRequest request)
        {
            var result = await _authenticationService.InitialPasswordSetAsync(request.StaffCode, request.DefaultPassword, request.NewPassword);
            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new InitialPasswordSetResponse
            {
                Success = result.success,
                Message = result.message
            });
        }

        // POST: api/Authentication/ResetForgottenPassword
        [HttpPost("ResetForgottenPassword")]
        public async Task<IActionResult> ResetForgottenPassword([FromBody] ResetForgottenPasswordRequest request)
        {
            var result = await _authenticationService.ResetForgottenPasswordAsync(request.StaffCode, request.TemporaryPassword, request.NewPassword);
            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new ResetForgottenPasswordResponse
            {
                Success = result.success,
                Message = result.message
            });
        }
    }
}
