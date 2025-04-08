using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMFDomain.Entities.Staff;

namespace TMFDomain.Interfaces.Staff
{
    public interface IPositionRepository
    {
        // Get a position by its ID
        Task<Position> GetByIdAsync(int id);

        // Get all positions
        Task<IEnumerable<Position>> GetAllPositionsAsync();

        // Add a new position
        Task<Position> AddAsync(Position position);

        // Update an existing position
        Task UpdateAsync(Position position);

        // Check if a position is in use (e.g., assigned to any staff member)
        Task<bool> IsInUseAsync(int id);

        // Delete a position if it is not in use (hard delete)
        Task<bool> DeletePositionIfNotInUseAsync(int id);
    }
}