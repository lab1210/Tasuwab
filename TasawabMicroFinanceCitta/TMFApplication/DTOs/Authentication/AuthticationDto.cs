using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMFApplication.DTOs.Authentication
{
    public class LoginRequest
    {
        public string EmailOrStaffCode { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }

        // Additional user details
        public string StaffCode { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PositionCode { get; set; }
        public string DepartmentCode { get; set; }
        public string BranchCode { get; set; }

        // Staff personal information
        public StaffPersonalInformationResponse StaffPersonalInformation { get; set; }
    }

    public class StaffPersonalInformationResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MartialStatus { get; set; }
        public int? DateOfBirth { get; set; } // YYYYMMDD format
        public string Address { get; set; }
        public string Phone { get; set; }
        public string StaffImage { get; set; }
    }


    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class SetPasswordRequest
    {
        public string StaffCode { get; set; }
        public string DefaultPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class SetPasswordResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class InitialPasswordSetRequest
    {
        public string StaffCode { get; set; }
        public string DefaultPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class InitialPasswordSetResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    // New DTO for resetting forgotten password
    public class ResetForgottenPasswordRequest
    {
        public string StaffCode { get; set; }
        public string TemporaryPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetForgottenPasswordResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
