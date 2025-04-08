using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMFDomain.Entities.Authentication;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Authentication;
using TMFDomain.Interfaces.Staff;
using TMFApplication.Utilities;
using Microsoft.Extensions.Configuration;

namespace TMFInfrastructure.Data.Seeders
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IStaffPersonalInformationRepository _staffRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IConfiguration _configuration;

        public DataSeeder(
            ApplicationDbContext context,
            IUserRepository userRepository,
            IStaffPersonalInformationRepository staffRepository,
            IRoleRepository roleRepository,
            IBranchRepository branchRepository,
            IDepartmentRepository departmentRepository,
            IPositionRepository positionRepository,
            IConfiguration configuration)
        {
            _context = context;
            _userRepository = userRepository;
            _staffRepository = staffRepository;
            _roleRepository = roleRepository;
            _branchRepository = branchRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _configuration = configuration;
        }

        public async Task SeedDataAsync()
        {
            // Check if seeding is enabled in appsettings
            var seedEnabled = _configuration.GetValue<bool>("SeedData:Enabled");
            if (!seedEnabled) return;

            await _context.Database.MigrateAsync();

            // Create default branch if it doesn't exist
            var defaultBranch = await _branchRepository.GetByIdAsync(1);
            if (defaultBranch == null)
            {
                defaultBranch = new Branch
                {
                    name = "Head Office",
                    description = "Main headquarters branch",
                    email = "headoffice@tasawabfinance.com",
                    phone = "+1234567890",
                    is_active = true,
                    is_visible = true
                };
                await _branchRepository.AddAsync(defaultBranch);
            }

            // Create default department if it doesn't exist
            var defaultDepartment = await _departmentRepository.GetByIdAsync(1);
            if (defaultDepartment == null)
            {
                defaultDepartment = new Department
                {
                    name = "Administration",
                    description = "Administrative department",
                    email = "admin@tasawabfinance.com",
                    phone = "+1234567890",
                    is_active = true,
                    is_visible = true
                };
                await _departmentRepository.AddAsync(defaultDepartment);
            }

            // Create default position if it doesn't exist
            var defaultPosition = await _positionRepository.GetByIdAsync(1);
            if (defaultPosition == null)
            {
                defaultPosition = new Position
                {
                    name = "System Administrator",
                    description = "Responsible for system administration"
                };
                await _positionRepository.AddAsync(defaultPosition);
            }

            // Seed all privileges first
            await SeedPrivilegesAsync();

            // Create admin role if it doesn't exist
            var adminRole = await _roleRepository.GetByRoleIdAsync("ADMIN");
            if (adminRole == null)
            {
                adminRole = new Role
                {
                    role_id = "ADMIN",
                    name = "Administrator",
                    description = "System administrator with full access"
                };

                // Get all privileges except P028-P038
                var allPrivileges = await _roleRepository.GetAllPrivilegesAsync();
                var adminPrivileges = allPrivileges
                    .Where(p => !new[] { "P028", "P029", "P030", "P031", "P032", "P033", "P034", "P035", "P036", "P037", "P038","P040" }
                                .Contains(p.PrivilegeId))
                    .ToList();

                adminRole.Privileges.AddRange(adminPrivileges);
                await _roleRepository.AddAsync(adminRole);
            }
            else
            {
                // Get current privileges and filter out P028-P038
                var currentPrivilegeIds = adminRole.Privileges
                    .Select(p => p.PrivilegeId)
                    .ToList();

                var allPrivileges = await _roleRepository.GetAllPrivilegesAsync();
                var adminPrivileges = allPrivileges
                    .Where(p => !new[] { "P028", "P029", "P030", "P031", "P032", "P033", "P034", "P035", "P036", "P037", "P038","P040" }
                                .Contains(p.PrivilegeId))
                    .ToList();

                // Remove any P028-P038 privileges that might have been added before
                adminRole.Privileges.RemoveAll(p => new[] { "P028", "P029", "P030", "P031", "P032", "P033", "P034", "P035", "P036", "P037", "P038","P040" }
                                            .Contains(p.PrivilegeId));

                // Add any missing non-P028-P038 privileges
                foreach (var privilege in adminPrivileges)
                {
                    if (!currentPrivilegeIds.Contains(privilege.PrivilegeId))
                    {
                        adminRole.Privileges.Add(privilege);
                    }
                }

                await _roleRepository.UpdateAsync(adminRole);
            }

            // Check if admin user already exists
            var adminStaffCode = "ADMIN001";
            var adminUser = await _userRepository.GetByStaffCodeAsync(adminStaffCode);
            if (adminUser == null)
            {
                // Create staff personal information
                var adminStaff = new StaffPersonalInformation
                {
                    staff_code = adminStaffCode,
                    first_name = "System",
                    last_name = "Administrator",
                    gender = "Female",
                    martial_status = "Single",
                    date_of_birth = 19800101,
                    address = "Head Office",
                    email = "admin@tasawabfinance.com",
                    phone = "+2344567890"
                };
                await _staffRepository.AddAsync(adminStaff);

                var adminPassword = _configuration["SeedData:AdminPassword"] ?? "admin123";
                var hashedPassword = PasswordHelper.HashPassword(adminPassword);

                adminUser = new User
                {
                    staff_code = adminStaffCode,
                    email = "admin@tasawabfinance.com",
                    role_code = adminRole.role_id,
                    position_code = defaultPosition.position_id.ToString(),
                    department_code = defaultDepartment.department_id.ToString(),
                    branch_code = defaultBranch.branch_id.ToString(),
                    password = hashedPassword,
                    IsPasswordSet = true,
                    is_active = true,
                    IsVisible = true
                };
                await _userRepository.AddAsync(adminUser);
            }
        }

        private async Task SeedPrivilegesAsync()
        {
            var privilegesToSeed = new List<Privilege>
            {
                new Privilege { PrivilegeId = "P001", Name = "CreateStaff", Description = "Allows creating of staffs." },
                new Privilege { PrivilegeId = "P002", Name = "UpdateStaff", Description = "Allows updating of staff information." },
                new Privilege { PrivilegeId = "P003", Name = "DeleteStaff", Description = "Allows deleting of staff members." },
                new Privilege { PrivilegeId = "P004", Name = "ViewStaffs", Description = "Allows viewing of all staff members." },
                new Privilege { PrivilegeId = "P005", Name = "ActivateStaff", Description = "Allows activating a deactivated staff member." },
                new Privilege { PrivilegeId = "P006", Name = "DeactivateStaff", Description = "Allows deactivating a staff member." },
                new Privilege { PrivilegeId = "P007", Name = "AddBranch", Description = "Allows adding new branches." },
                new Privilege { PrivilegeId = "P008", Name = "UpdateBranch", Description = "Allows updating branch information." },
                new Privilege { PrivilegeId = "P009", Name = "ViewBranch", Description = "Allows viewing branch details." },
                new Privilege { PrivilegeId = "P010", Name = "ActivateBranch", Description = "Allows activating a branch." },
                new Privilege { PrivilegeId = "P011", Name = "DeactivateBranch", Description = "Allows deactivating a branch." },
                new Privilege { PrivilegeId = "P012", Name = "DeleteBranch", Description = "Allows deleting a branch." },
                new Privilege { PrivilegeId = "P013", Name = "ViewRoles", Description = "Allows viewing roles." },
                new Privilege { PrivilegeId = "P014", Name = "CreateRole", Description = "Allows creating new roles." },
                new Privilege { PrivilegeId = "P015", Name = "UpdateRole", Description = "Allows updating role information." },
                new Privilege { PrivilegeId = "P016", Name = "DeleteRole", Description = "Allows deleting roles." },
                new Privilege { PrivilegeId = "P017", Name = "ViewPrivileges", Description = "Allows viewing privileges." },
                new Privilege { PrivilegeId = "P018", Name = "AddDepartment", Description = "Allows adding new departments." },
                new Privilege { PrivilegeId = "P019", Name = "UpdateDepartment", Description = "Allows updating department information." },
                new Privilege { PrivilegeId = "P020", Name = "ActivateDepartment", Description = "Allows activating departments." },
                new Privilege { PrivilegeId = "P021", Name = "DeactivateDepartment", Description = "Allows deactivating departments." },
                new Privilege { PrivilegeId = "P022", Name = "DeleteDepartment", Description = "Allows deleting departments." },
                new Privilege { PrivilegeId = "P023", Name = "ViewDepartments", Description = "Allows viewing departments." },
                new Privilege { PrivilegeId = "P024", Name = "AddPosition", Description = "Allows adding new positions." },
                new Privilege { PrivilegeId = "P025", Name = "UpdatePosition", Description = "Allows updating position information." },
                new Privilege { PrivilegeId = "P026", Name = "DeletePosition", Description = "Allows deleting positions." },
                new Privilege { PrivilegeId = "P027", Name = "ViewPositions", Description = "Allows viewing positions." },
                new Privilege { PrivilegeId = "P028", Name = "ViewClients", Description = "Allows viewing clients." },
                new Privilege { PrivilegeId = "P029", Name = "UpdateClients", Description = "Allows updating client information." },
                new Privilege { PrivilegeId = "P030", Name = "CreateClients", Description = "Allows creating new clients." },
                new Privilege { PrivilegeId = "P031", Name = "PostTransaction", Description = "Allows posting transactions." },
                new Privilege { PrivilegeId = "P032", Name = "ViewTransation", Description = "Allows viewing transactions." },
                new Privilege { PrivilegeId = "P033", Name = "ViewTransactionCharge", Description = "Allows viewing transaction charges." },
                new Privilege { PrivilegeId = "P034", Name = "UpdateTransactionCharge", Description = "Allows updating transaction charges." },
                new Privilege { PrivilegeId = "P035", Name = "CreateAccount", Description = "Allows creating accounts." },
                new Privilege { PrivilegeId = "P036", Name = "UpdateAccount", Description = "Allows updating account information." },
                new Privilege { PrivilegeId = "P037", Name = "ViewAccount", Description = "Allows viewing accounts." },
                new Privilege { PrivilegeId = "P038", Name = "AccountMetaData", Description = "Allows accessing account metadata." },
                new Privilege { PrivilegeId = "P039", Name = "AccountMetaDataAdmin", Description = "Allows handling account metadata." },
                new Privilege { PrivilegeId = "P040", Name = "CreateTransactionCharge", Description = "Creating Transaction Charge" }


            };

            foreach (var privilege in privilegesToSeed)
            {
                var existingPrivilege = await _roleRepository.GetPrivilegeByIdAsync(privilege.PrivilegeId);
                if (existingPrivilege == null)
                {
                    privilege.created_at = DateTime.Now;
                    privilege.updated_at = DateTime.Now;
                    await _context.Privileges.AddAsync(privilege);
                }
                else
                {
                    existingPrivilege.Name = privilege.Name;
                    existingPrivilege.Description = privilege.Description;
                    existingPrivilege.updated_at = DateTime.Now;
                    _context.Privileges.Update(existingPrivilege);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}