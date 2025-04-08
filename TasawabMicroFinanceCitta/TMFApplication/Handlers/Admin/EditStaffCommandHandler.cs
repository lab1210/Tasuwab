using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TMFApplication.Contracts.Admin.Commands;
using TMFApplication.DTOs.Admin;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Staff;
using TMFDomain.Entities.Authentication;
using TMFDomain.Interfaces.Authentication;

namespace TMFApplication.Handlers.Admin
{
    public class EditStaffCommandHandler : IRequestHandler<EditStaffCommand, EditStaffResponse>
    {
        private readonly IStaffPersonalInformationRepository _staffRepository;
        private readonly IUserRepository _userRepository;

        public EditStaffCommandHandler(
            IStaffPersonalInformationRepository staffRepository,
            IUserRepository userRepository)
        {
            _staffRepository = staffRepository;
            _userRepository = userRepository;
        }

        public async Task<EditStaffResponse> Handle(EditStaffCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing staff information
                var staff = await _staffRepository.GetByStaffCodeAsync(request.Request.StaffCode);
                if (staff == null)
                {
                    return new EditStaffResponse
                    {
                        Success = false,
                        Message = "Staff not found."
                    };
                }

                // Update staff personal information
                staff.first_name = request.Request.FirstName;
                staff.last_name = request.Request.LastName;
                staff.gender = request.Request.Gender;
                staff.martial_status = request.Request.MartialStatus;
                staff.date_of_birth = request.Request.DateOfBirth;
                staff.address = request.Request.Address;
                staff.email = request.Request.Email.Value; // Assuming Email is a ValueObject with Value property
                staff.phone = request.Request.Phone;
                staff.staff_image = request.Request.StaffImage;
                staff.updated_at = DateTime.Now;

                // Update user information (role, position, department, branch)
                var user = await _userRepository.GetByStaffCodeAsync(request.Request.StaffCode);
                if (user != null)
                {
                    user.role_code = request.Request.RoleID;
                    user.position_code = request.Request.PositionID;
                    user.department_code = request.Request.DepartmentID;
                    user.branch_code = request.Request.BranchID;
                    user.updated_at = DateTime.Now;

                    await _userRepository.UpdateUserAsync(user);
                }

                // Save changes
                await _staffRepository.UpdateAsync(staff);

                return new EditStaffResponse
                {
                    Success = true,
                    Message = "Staff updated successfully."
                };
            }
            catch (Exception ex)
            {
                // Log the exception here
                return new EditStaffResponse
                {
                    Success = false,
                    Message = $"An error occurred while updating staff: {ex.Message}"
                };
            }
        }
    }
}