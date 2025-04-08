using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.DTOs.Authentication;
using TMFApplication.Utilities;
using TMFDomain.Entities.Authentication;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces;
using TMFDomain.Interfaces.Authentication;
using TMFDomain.Interfaces.Staff;

namespace TMFApplication.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IStaffPersonalInformationRepository _staffPersonalInformationRepository;
        private readonly IConfiguration _configuration;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService(
             IUserRepository userRepository,
             IEmailService emailService,
             IStaffPersonalInformationRepository staffPersonalInformationRepository,
             IRoleRepository roleRepository,
             IJwtTokenService jwtTokenService,
             IHttpContextAccessor httpContextAccessor,
             IConfiguration configuration)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _staffPersonalInformationRepository = staffPersonalInformationRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<(bool success, string message, string token, User user, StaffPersonalInformation staffPersonalInfo)> LoginAsync(string emailOrStaffCode, string password)
        {
            var user = await _userRepository.GetByEmailOrStaffCodeAsync(emailOrStaffCode);

            if (user == null || user.is_active != true || user.IsVisible != true)
                return (false, "Invalid credentials or user is deactivated/deleted", null, null, null);

            if (!user.IsPasswordSet)
                return (false, "Password not set. Please reset your password.", null, null, null);

            if (!PasswordHelper.VerifyPassword(password, user.password))
                return (false, "Invalid credentials", null, null, null);

            // Generate access token
            var token = await _jwtTokenService.GenerateAccessTokenAsync(user);

            // Generate refresh token (stateless - signed JWT)
            var refreshToken = GenerateStatelessRefreshToken(user.staff_code);

            Console.WriteLine($"Setting refresh token cookie: {refreshToken}");
            SetRefreshTokenCookie(refreshToken, DateTime.UtcNow.AddDays(_jwtTokenService.RefreshTokenExpiryDays));

            var staffPersonalInfo = await _staffPersonalInformationRepository.GetByStaffCodeAsync(user.staff_code);

            return (true, "Login successful", token, user, staffPersonalInfo);
        }
        public async Task<(bool success, string message, string token)> RefreshTokenAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return (false, "Refresh token is required", null);

            // Validate the refresh token statelessly
            var principal = _jwtTokenService.ValidateRefreshToken(refreshToken);
            if (principal == null)
                return (false, "Invalid or expired refresh token", null);

            var staffCode = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(staffCode))
                return (false, "Invalid user context", null);

            // Verify user exists and is active
            var user = await _userRepository.GetByStaffCodeAsync(staffCode);
            if (user == null || user.is_active != true || user.IsVisible != true)
                return (false, "User not found or inactive", null);

            // Generate new access token
            var newAccessToken = await _jwtTokenService.GenerateAccessTokenAsync(user);

            // Generate new refresh token (rotation)
            var newRefreshToken = GenerateStatelessRefreshToken(user.staff_code);
            SetRefreshTokenCookie(newRefreshToken, DateTime.UtcNow.AddDays(_jwtTokenService.RefreshTokenExpiryDays));

            return (true, "Token refreshed successfully", newAccessToken);
        }
        public async Task<(bool success, string message)> LogoutAsync()
        {
            // Clear refresh token cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return (true, "Logout successful");
        }

        private string GenerateStatelessRefreshToken(string staffCode)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Token"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, staffCode),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(_jwtTokenService.RefreshTokenExpiryDays),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private void SetRefreshTokenCookie(string refreshToken, DateTime expiry)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expiry,
                Secure = true, // Set to false if testing on HTTP
                SameSite = SameSiteMode.Strict,
                Path = "/" // Changed from "/api/auth/refresh"
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        // Reset password and send temporary password via email
        public async Task<(bool success, string message)> ResetPasswordAsync(string email)
        {
            // Fetch the user by email
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return (false, "User not found");

            // Generate a temporary password and hash it
            var temporaryPassword = PasswordHelper.GenerateRandomPassword();
            var hashedPassword = PasswordHelper.HashPassword(temporaryPassword);

            // Update the user's password and set IsPasswordSet to false
            await _userRepository.UpdatePasswordAsync(user.staff_code, hashedPassword);
            await _userRepository.SetPasswordAsync(user.staff_code, false);

            // Send the password reset email with the staff code and temporary password
            await _emailService.SendPasswordResetEmailAsync(email, user.staff_code, temporaryPassword);

            return (true, "Password reset successful. Check your email for the temporary password.");
        }

        // Set new password after verifying default password
        public async Task<(bool success, string message)> SetPasswordAsync(string staffCode, string defaultPassword, string newPassword)
        {
            // This method remains the same as the original implementation
            // Fetch the user by staff code
            var user = await _userRepository.GetByStaffCodeAsync(staffCode);
            if (user == null)
                return (false, "User not found");

            // Check if the user has already set a password
            if (user.IsPasswordSet)
                return (false, "Password already set. Please use the reset password functionality.");

            // Verify the default password
            if (!PasswordHelper.VerifyPassword(defaultPassword, user.password))
                return (false, "Invalid default password");

            // Hash the new password
            var hashedPassword = PasswordHelper.HashPassword(newPassword);

            // Update the password and set IsPasswordSet to true
            await _userRepository.UpdatePasswordAsync(staffCode, hashedPassword);
            await _userRepository.SetPasswordAsync(staffCode, true);

            return (true, "Password set successfully");
        }

        public async Task<(bool success, string message)> InitialPasswordSetAsync(string staffCode, string defaultPassword, string newPassword)
        {
            // This is essentially the same as the current SetPasswordAsync
            // Kept for clarity and potential future differentiation
            return await SetPasswordAsync(staffCode, defaultPassword, newPassword);
        }

        public async Task<(bool success, string message)> ResetForgottenPasswordAsync(string staffCode, string temporaryPassword, string newPassword)
        {
            // Fetch the user by staff code
            var user = await _userRepository.GetByStaffCodeAsync(staffCode);
            if (user == null)
                return (false, "User not found");

            // Check if the user's password is not set (which happens after password reset)
            if (user.IsPasswordSet)
                return (false, "Cannot use this method. Use standard password reset.");

            // Verify the temporary password
            if (!PasswordHelper.VerifyPassword(temporaryPassword, user.password))
                return (false, "Invalid temporary password");

            // Hash the new password
            var hashedPassword = PasswordHelper.HashPassword(newPassword);

            // Update the password and set IsPasswordSet to true
            await _userRepository.UpdatePasswordAsync(staffCode, hashedPassword);
            await _userRepository.SetPasswordAsync(staffCode, true);

            return (true, "Password reset successfully");
        }

    
    }
}