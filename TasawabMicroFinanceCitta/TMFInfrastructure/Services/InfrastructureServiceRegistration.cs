using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMFApplication.Handlers.Admin;
using TMFApplication.Handlers.Authentication;
using TMFApplication.Services.Account;
using TMFApplication.Services.Admin;
using TMFApplication.Services.Authentication;
using TMFApplication.Services.Client;
using TMFApplication.Services.Transaction;
using TMFDomain.Interfaces;
using TMFDomain.Interfaces.Account;
using TMFDomain.Interfaces.Authentication;
using TMFDomain.Interfaces.Client;
using TMFDomain.Interfaces.Staff;
using TMFDomain.Interfaces.Transaction;
using TMFInfrastructure.Data;
using TMFInfrastructure.Data.Seeders;
using TMFInfrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace TMFInfrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {


            // Database connection
            string? connectionString = "Server=LABAKE\\SQLEXPRESS;Database=Tasuwabloan;Trusted_Connection=True;TrustServerCertificate=True";

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b =>
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStaffPersonalInformationRepository, StaffPersonalInformationRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IStaffPersonalInformationRepository, StaffPersonalInformationRepository>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            // Register new repositories
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();


            // Register the background service as a singleton
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(EditStaffCommandHandler).Assembly));

            // Register services
            services.AddScoped<StaffService>();
            services.AddScoped<AccountService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<ClientService>();
            services.AddScoped<DataSeeder>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JwtSettings:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JwtSettings:Token"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteStaffCommandHandler).Assembly));
            // Add this to your InfrastructureServiceRegistration.cs
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ActivateStaffCommandHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(DeactivateStaffCommandHandler).Assembly);
                // Add other assemblies as needed
            });
            services.AddHttpContextAccessor();
            services.AddAuthorization(options =>
            {
                //staffs
                options.AddPolicy("CreateStaff", policy =>
                    policy.Requirements.Add(new PermissionRequirement("CreateStaff")));
                options.AddPolicy("UpdateStaff", policy =>
                    policy.Requirements.Add(new PermissionRequirement("UpdateStaff")));
                options.AddPolicy("ViewStaffs", policy =>
         policy.Requirements.Add(new PermissionRequirement("ViewStaffs")));
                options.AddPolicy("DeactivateStaff", policy =>
policy.Requirements.Add(new PermissionRequirement("DeactivateStaff")));
                options.AddPolicy("ActivateStaff", policy =>
policy.Requirements.Add(new PermissionRequirement("ActivateStaff")));
                options.AddPolicy("DeleteStaff", policy =>
policy.Requirements.Add(new PermissionRequirement("DeleteStaff")));

                //branches
                options.AddPolicy("AddBranch", policy =>
    policy.Requirements.Add(new PermissionRequirement("AddBranch")));
                options.AddPolicy("UpdateBranch", policy =>
                    policy.Requirements.Add(new PermissionRequirement("UpdateBranch")));
                options.AddPolicy("ViewBranch", policy =>
                    policy.Requirements.Add(new PermissionRequirement("ViewBranch")));
                options.AddPolicy("ActivateBranch", policy =>
                    policy.Requirements.Add(new PermissionRequirement("ActivateBranch")));
                options.AddPolicy("DeactivateBranch", policy =>
                    policy.Requirements.Add(new PermissionRequirement("DeactivateBranch")));
                options.AddPolicy("DeleteBranch", policy =>
                    policy.Requirements.Add(new PermissionRequirement("DeleteBranch")));
                //role
                options.AddPolicy("ViewRoles", policy =>
                    policy.Requirements.Add(new PermissionRequirement("ViewRoles")));
                options.AddPolicy("CreateRole", policy =>
                    policy.Requirements.Add(new PermissionRequirement("CreateRole")));
                options.AddPolicy("UpdateRole", policy =>
                    policy.Requirements.Add(new PermissionRequirement("UpdateRole")));
                options.AddPolicy("DeleteRole", policy =>
                    policy.Requirements.Add(new PermissionRequirement("DeleteRole")));
                options.AddPolicy("ViewPrivileges", policy =>
                    policy.Requirements.Add(new PermissionRequirement("ViewPrivileges")));
                //department
                options.AddPolicy("AddDepartment", policy =>
    policy.Requirements.Add(new PermissionRequirement("AddDepartment")));
                options.AddPolicy("UpdateDepartment", policy =>
                    policy.Requirements.Add(new PermissionRequirement("UpdateDepartment")));
                options.AddPolicy("ActivateDepartment", policy =>
                    policy.Requirements.Add(new PermissionRequirement("ActivateDepartment")));
                options.AddPolicy("DeactivateDepartment", policy =>
                    policy.Requirements.Add(new PermissionRequirement("DeactivateDepartment")));
                options.AddPolicy("DeleteDepartment", policy =>
                    policy.Requirements.Add(new PermissionRequirement("DeleteDepartment")));
                options.AddPolicy("ViewDepartments", policy =>
                    policy.Requirements.Add(new PermissionRequirement("ViewDepartments")));

                //position
                options.AddPolicy("AddPosition", policy =>
                    policy.Requirements.Add(new PermissionRequirement("AddPosition")));
                options.AddPolicy("UpdatePosition", policy =>
                    policy.Requirements.Add(new PermissionRequirement("UpdatePosition")));
                options.AddPolicy("DeletePosition", policy =>
                    policy.Requirements.Add(new PermissionRequirement("DeletePosition")));
                options.AddPolicy("ViewPositions", policy =>
                    policy.Requirements.Add(new PermissionRequirement("ViewPositions")));
                //clients
                options.AddPolicy("ViewClients", policy =>
    policy.Requirements.Add(new PermissionRequirement("ViewClients")));
                options.AddPolicy("UpdateClients", policy =>
                    policy.Requirements.Add(new PermissionRequirement("UpdateClients")));
                options.AddPolicy("CreateClients", policy =>
         policy.Requirements.Add(new PermissionRequirement("CreateClients")));

                //transaction
                options.AddPolicy("PostTransaction", policy =>
policy.Requirements.Add(new PermissionRequirement("PostTransaction")));
                options.AddPolicy("ViewTransation", policy =>
                    policy.Requirements.Add(new PermissionRequirement("ViewTransation")));
                options.AddPolicy("ViewTransactionCharge", policy =>
         policy.Requirements.Add(new PermissionRequirement("ViewTransactionCharge")));
                options.AddPolicy("UpdateTransactionCharge", policy =>
        policy.Requirements.Add(new PermissionRequirement("UpdateTransactionCharge")));
                options.AddPolicy("CreateTransactionCharge", policy =>
        policy.Requirements.Add(new PermissionRequirement("CreateTransactionCharge")));

                //accounts
                options.AddPolicy("CreateAccount", policy =>
policy.Requirements.Add(new PermissionRequirement("CreateAccount")));
                options.AddPolicy("UpdateAccount", policy =>
                    policy.Requirements.Add(new PermissionRequirement("UpdateAccount")));
                options.AddPolicy("ViewAccount", policy =>
         policy.Requirements.Add(new PermissionRequirement("ViewAccount")));
                options.AddPolicy("ViewAccount", policy =>
        policy.Requirements.Add(new PermissionRequirement("ViewAccount")));
                options.AddPolicy("AccountMetaData", policy =>
        policy.Requirements.Add(new PermissionRequirement("AccountMetaData")));
                options.AddPolicy("AccountMetaDataAdmin", policy =>
policy.Requirements.Add(new PermissionRequirement("AccountMetaDataAdmin")));

            });

            // Register AuthenticationService
            services.AddScoped<IAuthenticationService, AuthenticationService>(); ;

            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly));

            return services;


        }
    }
}