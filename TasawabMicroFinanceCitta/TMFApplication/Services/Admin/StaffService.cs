using System;
using System.Threading.Tasks;
using TMFApplication.DTOs.Admin;
using TMFApplication.Utilities;
using TMFDomain.Entities.Authentication;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Authentication;
using TMFDomain.Interfaces.Staff;
using TMFDomain.ValueObjects;
using TMFDomain.ValueObjects.TMFDomain.ValueObjects;

namespace TMFApplication.Services.Admin
{
    public class StaffService
    {
        private readonly IStaffPersonalInformationRepository _staffPersonalInformationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IRoleRepository _roleRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPositionRepository _positionRepository;

        public StaffService(
            IStaffPersonalInformationRepository staffPersonalInformationRepository,
            IUserRepository userRepository,
            IEmailService emailService,
            IRoleRepository roleRepository,
            IBranchRepository branchRepository,
            IDepartmentRepository departmentRepository,
            IPositionRepository positionRepository)
        {
            _staffPersonalInformationRepository = staffPersonalInformationRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _roleRepository = roleRepository;
            _branchRepository = branchRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
        }
        public async Task<CreateStaffResponse> CreateStaffAsync(CreateStaffRequest request)
        {
            try
            {
                // Validate the email format using the Email value object
                var email = new Email(request.Email.Value);

                // Validate if the email is unique
                if (!await _staffPersonalInformationRepository.IsEmailUniqueAsync(email.Value))
                {
                    return new CreateStaffResponse
                    {
                        Success = false,
                        Message = "Email is already in use."
                    };
                }

                // Validate if the role exists
                var role = await _roleRepository.GetByRoleIdAsync(request.RoleID);
                if (role == null)
                {
                    return new CreateStaffResponse
                    {
                        Success = false,
                        Message = "Role does not exist."
                    };
                }

                // Generate a unique staff code
                string staffCode;
                do
                {
                    staffCode = StaffCodeGenerator.GenerateStaffCode();
                } while (!await _staffPersonalInformationRepository.IsStaffCodeUniqueAsync(staffCode));

                // Generate a temporary password
                var temporaryPassword = PasswordHelper.GenerateRandomPassword();
                var hashedPassword = PasswordHelper.HashPassword(temporaryPassword);

                // Create Staff Personal Information
                var staffPersonalInfo = new StaffPersonalInformation
                {
                    staff_code = staffCode, // Assign the generated staff code
                    first_name = request.FirstName,
                    last_name = request.LastName,
                    gender = request.Gender,
                    martial_status = request.MartialStatus,
                    date_of_birth = request.DateOfBirth,
                    address = request.Address,
                    email = email.Value, // Use the validated email
                    phone = request.Phone,
                    staff_image = request.StaffImage
                };

                await _staffPersonalInformationRepository.AddAsync(staffPersonalInfo);

                // Create User Account
                var user = new User
                {
                    staff_code = staffCode, // Use the same staff code
                    email = email.Value, // Use the validated email
                    role_code = role.role_id, // Use the role name
                    position_code = request.PositionID,
                    department_code = request.DepartmentID,
                    branch_code = request.BranchID,
                    password = hashedPassword,
                    IsPasswordSet = false,
                    is_active = true,
                    IsVisible = true
                };

                await _userRepository.AddAsync(user);

                // Send Email with Temporary Password and Staff Code
                bool emailSent = await _emailService.SendPasswordResetEmailAsync(email.Value, staffCode, temporaryPassword);

                if (!emailSent)
                {
                    // Include the reason for failure in the response
                    return new CreateStaffResponse
                    {
                        Success = false,
                        Message = $"Failed to send email. Reason: {_emailService.LastErrorMessage}"
                    };
                }

                return new CreateStaffResponse
                {
                    Success = true,
                    Message = "Staff created successfully. Email sent."
                };
            }
            catch (ArgumentException ex)
            {
                // Handle invalid email format
                return new CreateStaffResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<StaffWithUserResponse> GetStaffWithUserAsync(string staffCode)
        {
            var (staff, user) = await _staffPersonalInformationRepository.GetStaffWithUserAsync(staffCode);

            if (staff == null || user == null)
            {
                return new StaffWithUserResponse
                {
                    Success = false,
                    Message = "Staff or user account not found"
                };
            }

            // Map to DTOs
            var staffDto = new StaffInfoDto
            {
                StaffCode = staff.staff_code,
                FirstName = staff.first_name,
                LastName = staff.last_name,
                Gender = staff.gender,
                MartialStatus = staff.martial_status,
                DateOfBirth = staff.date_of_birth,
                Address = staff.address,
                Email = staff.email,
                Phone = staff.phone,
                StaffImage = staff.staff_image
            };

            var userDto = new UserInfoDto
            {
                //StaffCode = user.staff_code,
                Email = user.email,
                RoleCode = user.role_code,
                PositionCode = user.position_code,
                DepartmentCode = user.department_code,
                BranchCode = user.branch_code,
                IsActive = user.is_active ?? false,
                IsPasswordSet = user.IsPasswordSet
            };

            return new StaffWithUserResponse
            {
                Success = true,
                Message = "Staff and user information retrieved",
                Staff = staffDto,
                User = userDto
            };
        }

        public async Task<DeactivateStaffResponse> DeactivateStaffAsync(string staffCode)
        {
            var user = await _userRepository.GetByStaffCodeAsync(staffCode);
            if (user == null)
            {
                return new DeactivateStaffResponse
                {
                    Success = false,
                    Message = "Staff not found."
                };
            }

            user.is_active = false;
            await _userRepository.UpdateAsync(user);

            return new DeactivateStaffResponse
            {
                Success = true,
                Message = "Staff deactivated successfully."
            };
        }

        public async Task<ActivateStaffResponse> ActivateStaffAsync(string staffCode)
        {
            var user = await _userRepository.GetByStaffCodeAsync(staffCode);
            if (user == null)
            {
                return new ActivateStaffResponse
                {
                    Success = false,
                    Message = "Staff not found."
                };
            }

            user.is_active = true;
            await _userRepository.UpdateAsync(user);

            return new ActivateStaffResponse
            {
                Success = true,
                Message = "Staff activated successfully."
            };
        }

        // Delete Staff (Set IsVisible to false)
        public async Task<DeleteStaffResponse> DeleteStaffAsync(string staffCode)
        {
            var user = await _userRepository.GetByStaffCodeAsync(staffCode);
            if (user == null)
            {
                return new DeleteStaffResponse
                {
                    Success = false,
                    Message = "Staff not found."
                };
            }

            user.IsVisible = false;
            await _userRepository.UpdateAsync(user);

            return new DeleteStaffResponse
            {
                Success = true,
                Message = "Staff deleted successfully."
            };
        }
        public async Task<GetStaffsResponse> GetStaffsAsync()
        {
            var staffs = await _staffPersonalInformationRepository.GetAllAsync();
            var users = await _userRepository.GetAllAsync();

            var staffDetails = staffs.Join(users.Where(u => u.IsVisible == true), // Add this filter
                staff => staff.staff_code,
                user => user.staff_code,
                (staff, user) => new StaffDetails
                {
                    StaffCode = staff.staff_code,
                    FirstName = staff.first_name,
                    LastName = staff.last_name,
                    BranchID = int.TryParse(user.branch_code, out int branchId)
                        ? _branchRepository.GetByIdAsync(branchId).Result?.name
                        : null,
                    DepartmentID = int.TryParse(user.department_code, out int departmentId)
                        ? _departmentRepository.GetByIdAsync(departmentId).Result?.name
                        : null,
                    PositionID = int.TryParse(user.position_code, out int positionId)
                        ? _positionRepository.GetByIdAsync(positionId).Result?.name
                        : null,
                    RoleID = _roleRepository.GetByRoleIdAsync(user.role_code).Result?.name,
                    IsActive = user.is_active ?? false
                })
                .ToList();

            return new GetStaffsResponse
            {
                Success = true,
                Message = "Staff details retrieved successfully.",
                Staffs = staffDetails
            };
        }

        public async Task<EditStaffResponse> EditStaffAsync(EditStaffRequest request)
        {
            var staff = await _staffPersonalInformationRepository.GetByStaffCodeAsync(request.StaffCode);
            if (staff == null)
            {
                return new EditStaffResponse
                {
                    Success = false,
                    Message = "Staff not found."
                };
            }

            // Update staff details
            staff.first_name = request.FirstName;
            staff.last_name = request.LastName;
            staff.gender = request.Gender;
            staff.martial_status = request.MartialStatus;
            staff.date_of_birth = request.DateOfBirth;
            staff.address = request.Address;
            staff.email = request.Email.Value;
            staff.phone = request.Phone;
            staff.staff_image = request.StaffImage;

            await _staffPersonalInformationRepository.UpdateAsync(staff);

            // Update user details
            var user = await _userRepository.GetByStaffCodeAsync(request.StaffCode);
            if (user != null)
            {
                user.role_code = request.RoleID;
                user.position_code = request.PositionID;
                user.department_code = request.DepartmentID;
                user.branch_code = request.BranchID;

                await _userRepository.UpdateAsync(user);
            }

            return new EditStaffResponse
            {
                Success = true,
                Message = "Staff updated successfully."
            };
        }
    }
    }

