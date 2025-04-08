using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Staff;


namespace TMFDomain.Interfaces.Staff
{
    public interface IBranchRepository
    {
        Task<Branch> GetByIdAsync(int id);
        Task<IEnumerable<Branch>> GetAllAsync();
        Task<Branch> AddAsync(Branch branch);
        Task UpdateAsync(Branch branch);
        Task<bool> DeleteAsync(int id);
        Task ActivateAsync(int id);
        Task DeactivateAsync(int id);
    }
}
