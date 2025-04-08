using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMFDomain.Interfaces
{
    public interface IUserManagementService
    {
        Task<(bool success, string message)> CreateUserForStaffAsync(string staffCode, string initialPassword, string role);
        Task<(bool success, string message)> UpdateUserRoleAsync(string staffCode, string newRole);
        Task<(bool success, string message)> ResetPasswordAsync(string staffCode);
        Task<(bool success, string message)> ChangePasswordAsync(string staffCode, string currentPassword, string newPassword);
        Task<(bool success, string message, string token)> GeneratePasswordResetTokenAsync(string email);
        Task<(bool success, string message)> ValidatePasswordResetTokenAsync(string email, string token);
    }
}
