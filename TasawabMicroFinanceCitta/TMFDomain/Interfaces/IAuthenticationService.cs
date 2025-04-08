using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Authentication;
using TMFDomain.Entities.Staff;

namespace TMFDomain.Interfaces
{
    public interface IAuthenticationService
    {
        // Login and Token Management
        Task<(bool success, string message, string token, User user, StaffPersonalInformation staffPersonalInfo)> LoginAsync(string emailOrStaffCode, string password);
        Task<(bool success, string message, string token)> RefreshTokenAsync();
        Task<(bool success, string message)> LogoutAsync();

        // Password Management
        Task<(bool success, string message)> ResetPasswordAsync(string email);
        Task<(bool success, string message)> SetPasswordAsync(string staffCode, string defaultPassword, string newPassword);
        Task<(bool success, string message)> InitialPasswordSetAsync(string staffCode, string defaultPassword, string newPassword);
        Task<(bool success, string message)> ResetForgottenPasswordAsync(string staffCode, string temporaryPassword, string newPassword);
    }
}