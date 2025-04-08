using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMFDomain.Entities.Authentication;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Staff;
using TMFInfrastructure.Data;

namespace TMFInfrastructure.Repositories
{
    public class StaffPersonalInformationRepository : Repository<StaffPersonalInformation>, IStaffPersonalInformationRepository
    {
        public StaffPersonalInformationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<StaffPersonalInformation> GetByStaffCodeAsync(string staffCode)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.staff_code == staffCode);
        }

        public async Task<IEnumerable<StaffPersonalInformation>> GetByDepartmentAsync(string departmentId)
        {
            return await (from staff in _dbSet
                          join user in _context.Users on staff.staff_code equals user.staff_code
                          where user.department_code == departmentId
                          select staff).ToListAsync();
        }
        public async Task<(StaffPersonalInformation staff, User user)> GetStaffWithUserAsync(string staffCode)
        {
            var staff = await _dbSet.FirstOrDefaultAsync(s => s.staff_code == staffCode);
            if (staff == null) return (null, null);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.staff_code == staffCode && u.IsVisible == true);
            return (staff, user);
        }

        public async Task<IEnumerable<StaffPersonalInformation>> GetByBranchAsync(string branchId)
        {
            return await (from staff in _dbSet
                          join user in _context.Users on staff.staff_code equals user.staff_code
                          where user.branch_code == branchId
                          select staff).ToListAsync();
        }

        public async Task<IEnumerable<StaffPersonalInformation>> GetByPositionAsync(string positionId)
        {
            return await (from staff in _dbSet
                          join user in _context.Users on staff.staff_code equals user.staff_code
                          where user.position_code == positionId
                          select staff).ToListAsync();
        }

        public async Task UpdateAsync(StaffPersonalInformation staff)
        {
            _context.Entry(staff).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StaffPersonalInformation>> GetByRoleAsync(string roleId)
        {
            return await (from staff in _dbSet
                          join user in _context.Users on staff.staff_code equals user.staff_code
                          where user.role_code == roleId
                          select staff).ToListAsync();
        }

        public async Task<StaffPersonalInformation> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.email == email);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbSet.AnyAsync(s => s.email == email);
        }

        public async Task<bool> IsStaffCodeUniqueAsync(string staffCode)
        {
            return !await _dbSet.AnyAsync(s => s.staff_code == staffCode);
        }

        public async Task<bool> HasUserAccountAsync(string staffCode)
        {
            return await _context.Users.AnyAsync(u => u.staff_code == staffCode);
        }
    }
}