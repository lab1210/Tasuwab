using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Authentication;
using TMFDomain.Entities.Staff;

namespace TMFDomain.Interfaces.Staff
{
    public interface IStaffPersonalInformationRepository : IRepository<StaffPersonalInformation>
    {
        Task<StaffPersonalInformation> GetByStaffCodeAsync(string staffCode);
        Task<IEnumerable<StaffPersonalInformation>> GetByDepartmentAsync(string departmentId);
        Task<IEnumerable<StaffPersonalInformation>> GetByBranchAsync(string branchId);
        Task<IEnumerable<StaffPersonalInformation>> GetByPositionAsync(string positionId);
        Task<IEnumerable<StaffPersonalInformation>> GetByRoleAsync(string roleId);
        Task<StaffPersonalInformation> GetByEmailAsync(string email);
        Task UpdateAsync(StaffPersonalInformation staff);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsStaffCodeUniqueAsync(string staffCode);
        Task<bool> HasUserAccountAsync(string staffCode);
        Task<(StaffPersonalInformation staff, User user)> GetStaffWithUserAsync(string staffCode);
    }
}
