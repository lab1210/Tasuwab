using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Staff;
using TMFInfrastructure.Data;

namespace TMFInfrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments
                                .Where(d => d.is_visible) // Only include visible departments
                                .ToListAsync();
        }

        public async Task<Department> AddAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }
        public async Task DeactivateAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                department.is_active = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ActivateAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                department.is_active = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                department.is_visible = false; // Soft delete by setting is_visible to false
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> IsInUseAsync(int id)
        {
            // Check if any staff member is assigned to this department
            return await _context.Users.AnyAsync(s => s.department_code == id.ToString());
        }
    }
}