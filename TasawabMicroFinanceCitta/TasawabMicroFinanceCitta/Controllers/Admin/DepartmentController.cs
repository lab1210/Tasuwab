using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMFApplication.DTOs.Admin;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Staff;

namespace TasawabMicroFinanceCitta.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [Authorize(Policy = "AddDepartment")]
        [HttpPost("add")]
        public async Task<IActionResult> AddDepartment([FromBody] Department departmentDto)
        {
            var department = new Department
            {
                name = departmentDto.name,
                description = departmentDto.description,
                email = departmentDto.email,
                phone = departmentDto.phone,
                is_active = true,
                is_visible = true
            };

            await _departmentRepository.AddAsync(department);
            return Ok(new { success = true, message = "Department added successfully." });
        }
        [Authorize(Policy = "UpdateDepartment")]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditDepartment(int id, [FromBody] UpdateDepartmentDto departmentDto)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);
                if (department == null)
                {
                    return NotFound(new { success = false, message = "Department not found." });
                }

                if (!string.IsNullOrWhiteSpace(departmentDto.name))
                {
                    department.name = departmentDto.name;
                }
                if (!string.IsNullOrWhiteSpace(departmentDto.description))
                {
                    department.description = departmentDto.description;
                }
                if (!string.IsNullOrWhiteSpace(departmentDto.email))
                {
                    department.email = departmentDto.email;
                }
                if (!string.IsNullOrWhiteSpace(departmentDto.email))
                {
                    department.phone = departmentDto.phone;
                }

                await _departmentRepository.UpdateAsync(department);
                return Ok(new { success = true, message = "Department updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [Authorize(Policy = "ActivateDepartment")]
        [HttpPost("activate/{id}")]
        public async Task<IActionResult> ActivateDepartment(int id)
        {
            await _departmentRepository.ActivateAsync(id);
            return Ok(new { success = true, message = "Department activated successfully." });
        }
        [Authorize(Policy = "DeactivateDepartment")]
        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateDepartment(int id)
        {
            await _departmentRepository.DeactivateAsync(id);
            return Ok(new { success = true, message = "Department deactivated successfully." });
        }
        [Authorize(Policy = "DeleteDepartment")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            bool deleted = await _departmentRepository.DeleteAsync(id);
            if (!deleted)
            {
                return BadRequest(new { success = false, message = "Department is in use and cannot be deleted." });
            }

            return Ok(new { success = true, message = "Department deleted successfully." });
        }
        [Authorize(Policy = "ViewDepartments")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return Ok(departments);
        }
        [Authorize(Policy = "ViewDepartments")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound(new { success = false, message = "Department not found." });
            }

            return Ok(department);
        }
    }
}