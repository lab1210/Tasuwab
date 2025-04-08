using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Authentication;

namespace TMFDomain.Interfaces.Authentication
{

    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailOrStaffCodeAsync(string emailOrStaffCode);
        Task<User> GetByStaffCodeAsync(string staffCode);
        Task<User> GetByEmailAsync(string email);
        Task<bool> ValidateCredentialsAsync(string email, string password);
        Task UpdatePasswordAsync(string id, string newPassword);
        Task SetPasswordAsync(string staffCode, bool isPasswordSet);
        Task UpdateUserAsync(User user);

    }
}
