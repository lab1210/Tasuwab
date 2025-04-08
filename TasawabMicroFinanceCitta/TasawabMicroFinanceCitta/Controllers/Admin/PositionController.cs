using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMFApplication.DTOs.Admin;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Staff;

[ApiController]
[Route("api/[controller]")]
public class PositionController : ControllerBase
{
    private readonly IPositionRepository _positionRepository;

    public PositionController(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }
    [Authorize(Policy = "AddPosition")]
    [HttpPost("add")]
    public async Task<IActionResult> AddPosition([FromBody] CreatePositionDto positionDto)
    {
        var position = new Position
        {
            name = positionDto.name,
            description = positionDto.description
        };

        await _positionRepository.AddAsync(position);
        return Ok(new { success = true, message = "Position added successfully." });
    }
    [Authorize(Policy = "UpdatePosition")]
    [HttpPut("edit/{id}")]
    public async Task<IActionResult> EditPosition(int id, [FromBody] UpdatePositionDto positionDto)
    {
        var position = await _positionRepository.GetByIdAsync(id);
        if (position == null)
        {
            return NotFound(new { success = false, message = "Position not found." });
        }

        // Only update the name and description fields
        if (!string.IsNullOrWhiteSpace(positionDto.name))
        {
            position.name = positionDto.name;
        }

        if (!string.IsNullOrWhiteSpace(positionDto.description))
        {
            position.description = positionDto.description;
        }

        await _positionRepository.UpdateAsync(position);
        return Ok(new { success = true, message = "Position updated successfully." });
    }
    [Authorize(Policy = "DeletePosition")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeletePosition(int id)
    {
        bool deleted = await _positionRepository.DeletePositionIfNotInUseAsync(id);
        if (!deleted)
        {
            return BadRequest(new { success = false, message = "Position is in use and cannot be deleted." });
        }

        return Ok(new { success = true, message = "Position deleted successfully." });
    }
    [Authorize(Policy = "ViewPositions")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllPositions()
    {
        var positions = await _positionRepository.GetAllPositionsAsync();
        return Ok(positions);
    }
    [Authorize(Policy = "ViewPositions")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBranchById(int id)
    {
        var department = await _positionRepository.GetByIdAsync(id);
        if (department == null)
        {
            return NotFound(new { success = false, message = "Position not found." });
        }

        return Ok(department);
    }
}