using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Staff;
using TMFInfrastructure.Data;

namespace TMFInfrastructure.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly ApplicationDbContext _context;

        public PositionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Position> GetByIdAsync(int id)
        {
            return await _context.Positions.FindAsync(id);
        }

        public async Task<IEnumerable<Position>> GetAllPositionsAsync()
        {
            return await _context.Positions.ToListAsync();
        }

        public async Task<Position> AddAsync(Position position)
        {
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
            return position;
        }

        public async Task UpdateAsync(Position position)
        {
            _context.Positions.Update(position);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsInUseAsync(int id)
        {
           
            return await _context.Users.AnyAsync(s => s.position_code == id.ToString());
        }

        public async Task<bool> DeletePositionIfNotInUseAsync(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null) return false;

            if (await IsInUseAsync(id))
            {
                return false; 
            }

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return true; 
        }
    }
}