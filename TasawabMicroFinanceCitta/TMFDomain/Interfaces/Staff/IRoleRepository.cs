using System.Collections.Generic;
using System.Threading.Tasks;
using TMFDomain.Entities.Staff;

namespace TMFDomain.Interfaces.Staff
{
    public interface IRoleRepository
    {
        Task<Role> GetByRoleIdAsync(string roleId);
        Task<bool> IsInUseAsync(string roleId);
        Task<List<Role>> GetAllRolesAsync();
        Task<bool> DeleteRoleIfNotInUseAsync(string roleId);
        Task<Privilege> GetPrivilegeByIdAsync(string privilegeId);
        Task<List<Privilege>> GetAllPrivilegesAsync();
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
    }
}