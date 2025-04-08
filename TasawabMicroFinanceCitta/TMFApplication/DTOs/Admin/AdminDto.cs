using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TMFDomain.ValueObjects.TMFDomain.ValueObjects;

namespace TMFApplication.DTOs.Admin
{
    public class CreateStaffRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MartialStatus { get; set; }
        public int DateOfBirth { get; set; }
        public string Address { get; set; }
        public Email Email { get; set; }
        public string Phone { get; set; }
        public string StaffImage { get; set; }
        public string RoleID { get; set; }
        public string PositionID { get; set; }
        public string DepartmentID { get; set; }
        public string BranchID { get; set; }
    }
    public class CreateStaffResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class DeactivateStaffRequest
    {
        public string StaffCode { get; set; }
    }

    public class DeactivateStaffResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class DeleteStaffRequest
    {
        public string StaffCode { get; set; }
    }

    public class DeleteStaffResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class StaffDetails
    {
        public string StaffCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BranchID { get; set; }
        public string DepartmentID { get; set; }
        public string PositionID { get; set; }
        public string RoleID { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetStaffsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<StaffDetails> Staffs { get; set; }
    }
    public class CreateRoleDto
    {
        [Required]
        public string role_id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public List<string> PrivilegeIds { get; set; } = new List<string>();
    }

    public class UpdateRoleDto
    {
        [Required]
        public string role_id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public List<string> PrivilegeIds { get; set; } = new List<string>();
    }

    public class RoleResponse
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PrivilegeResponse> Privileges { get; set; } = new List<PrivilegeResponse>();
    }

    public class PrivilegeResponse
    {
        public string PrivilegeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class TaskDto
    {
        public string TaskId { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; } 
    }
    public class EditStaffRequest
    {
        public string StaffCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MartialStatus { get; set; }
        public int DateOfBirth { get; set; } // YYYYMMDD format
        public string Address { get; set; }
        public Email Email { get; set; }
        public string Phone { get; set; }
        public string StaffImage { get; set; }
        public string RoleID { get; set; }
        public string PositionID { get; set; }
        public string DepartmentID { get; set; }
        public string BranchID { get; set; }
    }

    public class EditStaffResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class CreateBranchDto
    {
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
    }

    public class UpdateBranchDto
    {
        public string? description { get; set; }
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
    }
    public class CreateDepartmentDto
    {
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
    }
    public class UpdateDepartmentDto
    {
        public string? name { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
    }
    public class CreatePositionDto
    {
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }
    public class UpdatePositionDto
    {
        public string? name { get; set; }
        public string? description { get; set; }

    }
    public class ActivateStaffResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class ActivateStaffRequest
    {
        public string StaffCode { get; set; }
    }

    public class StaffWithUserResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public StaffInfoDto Staff { get; set; }
        public UserInfoDto User { get; set; }
    }

    public class StaffInfoDto
    {
        public string StaffCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MartialStatus { get; set; }
        public int? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string StaffImage { get; set; }
    }

    public class UserInfoDto
    {
        //public string StaffCode { get; set; }
        public string Email { get; set; }
        public string RoleCode { get; set; }
        public string PositionCode { get; set; }
        public string DepartmentCode { get; set; }
        public string BranchCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsPasswordSet { get; set; }
    }
}
