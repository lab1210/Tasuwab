using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Staff;
using TMFInfrastructure.Data;

namespace TMFInfrastructure.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _context;

        public BranchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Implement GetByIdAsync
        public async Task<Branch> GetByIdAsync(int id)
        {
            return await _context.Branches.FindAsync(id);
        }

        // Implement GetAllAsync
        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _context.Branches
                                .Where(b => b.is_visible)
                                .ToListAsync();
        }

        //public async Task<IEnumerable<Branch>> GetSoftDeletedAsync()
        //{
        //    return await _context.Branches
        //                        .Where(b => !b.is_visible) // Only include soft-deleted branches
        //                        .ToListAsync();
        //}
        // Implement AddAsync

        // add name unique constraint to the db creation
        public async Task<Branch> AddAsync(Branch branch)
        {
            // Check if a branch with the same name already exists
            bool nameExists = await _context.Branches.AnyAsync(b => b.name == branch.name);
            if (nameExists)
            {
                throw new InvalidOperationException("A branch with the same name already exists.");
            }

            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

        // Implement UpdateAsync
        // add name unique constraint to the db creation
        public async Task UpdateAsync(Branch branch)
        {
            // Check if another branch with the same name already exists (excluding the current branch)
            if (branch.name != null)
            {
                bool nameExists = await _context.Branches.AnyAsync(b => b.name == branch.name && b.branch_id != branch.branch_id);
                if (nameExists)
                {
                    throw new InvalidOperationException("A branch with the same name already exists.");
                }
            }

            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
        }
        // Implement DeleteAsync
        public async Task<bool> DeleteAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                branch.is_visible = false;
                await _context.SaveChangesAsync();
                return true; 
            }
            return false;
        }
        // Implement ActivateAsync
        public async Task ActivateAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                branch.is_active = true;
                await _context.SaveChangesAsync();
            }
        }

        // Implement DeactivateAsync
        public async Task DeactivateAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                branch.is_active = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}