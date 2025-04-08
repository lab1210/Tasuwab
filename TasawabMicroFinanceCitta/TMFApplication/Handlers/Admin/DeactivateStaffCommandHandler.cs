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
    public class DeactivateStaffCommandHandler : IRequestHandler<DeactivateStaffCommand, DeactivateStaffResponse>
    {
        private readonly StaffService _staffService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeactivateStaffCommandHandler(StaffService staffService, IHttpContextAccessor httpContextAccessor)
        {
            _staffService = staffService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DeactivateStaffResponse> Handle(DeactivateStaffCommand request, CancellationToken cancellationToken)
        {
            // Get current user's staff code from claims
            var currentStaffCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Prevent self-deactivation
            if (currentStaffCode == request.Request.StaffCode)
            {
                return new DeactivateStaffResponse
                {
                    Success = false,
                    Message = "You cannot deactivate your own account."
                };
            }

            return await _staffService.DeactivateStaffAsync(request.Request.StaffCode);
        }
    }
}