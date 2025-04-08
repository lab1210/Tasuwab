using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Authentication;
using TMFDomain.Interfaces.Authentication;
using TMFInfrastructure.Data;

namespace TMFInfrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User> GetByStaffCodeAsync(string staffCode)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.staff_code == staffCode && u.IsVisible == true);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            // First find the staff with this email
            var staff = await _context.StaffPersonalInformation.FirstOrDefaultAsync(s => s.email == email);
            if (staff == null) return null;

            // Then find the user with that staff code
            return await _dbSet.FirstOrDefaultAsync(u => u.staff_code == staff.staff_code && u.IsVisible == true);

        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<User> GetByEmailOrStaffCodeAsync(string emailOrStaffCode)
        {
            return await _dbSet.FirstOrDefaultAsync(u => (u.email == emailOrStaffCode || u.staff_code == emailOrStaffCode) && u.IsVisible == true);
        }
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            // First check if this email exists in staff information
            var staff = await _context.StaffPersonalInformation.FirstOrDefaultAsync(s => s.email == email);
            if (staff == null) return true; // Email not found in staff, so it's unique

            // Then check if there's a user with this staff code
            return !await _dbSet.AnyAsync(u => u.staff_code == staff.staff_code && u.IsVisible == true);
        }

        public async Task DeactivateAsync(string id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                user.is_active = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ActivateAsync(string id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                user.is_active = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);
            if (user == null || user.is_active != true) return false;

            // In a real application, you'd hash the password and compare hashes
            // For this example, we're directly comparing passwords
            return user.password == password;
        }

        public async Task UpdatePasswordAsync(string staffCode, string newPassword)
        {
            var user = await GetByStaffCodeAsync(staffCode);
            if (user != null)
            {
                user.password = newPassword; // Ensure this is the hashed password
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetPasswordAsync(string staffCode, bool isPasswordSet)
        {
            var user = await GetByStaffCodeAsync(staffCode);
            if (user != null)
            {
                user.IsPasswordSet = isPasswordSet;
                await _context.SaveChangesAsync();
            }
        }


    }
}