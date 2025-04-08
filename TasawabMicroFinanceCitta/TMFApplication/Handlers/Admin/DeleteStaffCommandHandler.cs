using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TMFApplication.Contracts.Admin.Commands;
using TMFApplication.DTOs.Admin;
using TMFApplication.Services.Admin;

namespace TMFApplication.Handlers.Admin
{
    public class DeleteStaffCommandHandler : IRequestHandler<DeleteStaffCommand, DeleteStaffResponse>
    {
        private readonly StaffService _staffService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteStaffCommandHandler(StaffService staffService, IHttpContextAccessor httpContextAccessor)
        {
            _staffService = staffService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DeleteStaffResponse> Handle(DeleteStaffCommand command, CancellationToken cancellationToken)
        {
            // Get current user's staff code from claims
            var currentStaffCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Prevent self-deletion
            if (currentStaffCode == command.Request.StaffCode)
            {
                return new DeleteStaffResponse
                {
                    Success = false,
                    Message = "You cannot delete your own account."
                };
            }

            return await _staffService.DeleteStaffAsync(command.Request.StaffCode);
        }
    }
}