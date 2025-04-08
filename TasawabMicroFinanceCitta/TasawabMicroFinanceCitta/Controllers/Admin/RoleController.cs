using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Staff;
using TMFApplication.DTOs.Admin;

namespace TasawabMicroFinanceCitta.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        
        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        // [Authorize(Policy = "ViewRoles")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return Ok(roles);
        }
        [Authorize(Policy = "CreateRole")]
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            // Check if role_id is unique
            var existingRole = await _roleRepository.GetByRoleIdAsync(createRoleDto.role_id);
            if (existingRole != null)
            {
                return BadRequest(new { success = false, message = "Role ID already exists." });
            }
            
            var role = new Role
            {
                role_id = createRoleDto.role_id,
                name = createRoleDto.Name,
                description = createRoleDto.Description,
                Privileges = new List<Privilege>()
            };
            
            foreach (var privilegeId in createRoleDto.PrivilegeIds)
            {
                var privilege = await _roleRepository.GetPrivilegeByIdAsync(privilegeId);
                if (privilege != null)
                {
                    role.Privileges.Add(privilege);
                }
            }
            
            await _roleRepository.AddAsync(role);
            return Ok(new { success = true, message = "Role created successfully." });
        }
        [Authorize]
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            var role = await _roleRepository.GetByRoleIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new { success = false, message = "Role not found." });
            }
            return Ok(role);
        }
        [Authorize(Policy = "UpdateRole")]
        [HttpPut("update-role/{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId, [FromBody] UpdateRoleDto updateRoleDto)
        {
            var role = await _roleRepository.GetByRoleIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new { success = false, message = "Role not found." });
            }
            
            // Check if the new role_id is already in use (if it's changed)
            if (role.role_id != updateRoleDto.role_id)
            {
                var existingRole = await _roleRepository.GetByRoleIdAsync(updateRoleDto.role_id);
                if (existingRole != null)
                {
                    return BadRequest(new { success = false, message = "The new Role ID already exists." });
                }
            }
            
            role.role_id = updateRoleDto.role_id;
            role.name = updateRoleDto.Name;
            role.description = updateRoleDto.Description;
            
            // Clear existing privileges and add new ones
            role.Privileges.Clear();
            foreach (var privilegeId in updateRoleDto.PrivilegeIds)
            {
                var privilege = await _roleRepository.GetPrivilegeByIdAsync(privilegeId);
                if (privilege != null)
                {
                    role.Privileges.Add(privilege);
                }
            }
            
            await _roleRepository.UpdateAsync(role);
            return Ok(new { success = true, message = "Role updated successfully." });
        }
        [Authorize(Policy = "DeleteRole")]
        [HttpDelete("delete/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            bool deleted = await _roleRepository.DeleteRoleIfNotInUseAsync(roleId);
            if (!deleted)
            {
                return BadRequest(new { success = false, message = "Role is in use and cannot be deleted." });
            }
            return Ok(new { success = true, message = "Role deleted successfully." });
        }
        [Authorize(Policy = "ViewPrivileges")]
        [HttpGet("privileges/all")]
        public async Task<IActionResult> GetAllPrivileges()
        {
            var privileges = await _roleRepository.GetAllPrivilegesAsync();
            return Ok(privileges);
        }
    }
}