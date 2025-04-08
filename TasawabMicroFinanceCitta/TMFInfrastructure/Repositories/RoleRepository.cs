using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Staff;
using TMFInfrastructure.Data;

namespace TMFInfrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Role> GetByRoleIdAsync(string roleId)
        {
            return await _dbSet
                .Include(r => r.Privileges)
                .FirstOrDefaultAsync(r => r.role_id == roleId);
        }

        public async Task<bool> IsInUseAsync(string roleId)
        {
            var role = await GetByRoleIdAsync(roleId);
            if (role == null) return false;
            return await _context.Users.AnyAsync(u => u.role_code == role.role_id);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _dbSet.Include(r => r.Privileges).ToListAsync();
        }

        public async Task<bool> DeleteRoleIfNotInUseAsync(string roleId)
        {
            var role = await GetByRoleIdAsync(roleId);
            if (role == null) return false;
            if (await IsInUseAsync(roleId))
            {
                return false;
            }
            _dbSet.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Privilege> GetPrivilegeByIdAsync(string privilegeId)
        {
            return await _context.Privileges.FirstOrDefaultAsync(p => p.PrivilegeId == privilegeId);
        }

        public async Task<List<Privilege>> GetAllPrivilegesAsync()
        {
            return await _context.Privileges.ToListAsync();
        }
        //fix
        public async Task AddAsync(Role role)
        {
            await _dbSet.AddAsync(role);
            await _context.SaveChangesAsync();
        }
    }
}