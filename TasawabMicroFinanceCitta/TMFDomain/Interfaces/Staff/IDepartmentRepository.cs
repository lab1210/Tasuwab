using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMFDomain.Entities.Staff;

namespace TMFDomain.Interfaces.Staff
{
    public interface IDepartmentRepository
    {
        // Get a department by its ID
        Task<Department> GetByIdAsync(int id);
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeactivateAsync(int id);
        Task ActivateAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsInUseAsync(int id);
    }
}