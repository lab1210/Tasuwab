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
    public class ActivateStaffCommandHandler : IRequestHandler<ActivateStaffCommand, ActivateStaffResponse>
    {
        private readonly StaffService _staffService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActivateStaffCommandHandler(StaffService staffService, IHttpContextAccessor httpContextAccessor)
        {
            _staffService = staffService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ActivateStaffResponse> Handle(ActivateStaffCommand request, CancellationToken cancellationToken)
        {
            // Get current user's staff code from claims
            var currentStaffCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Prevent self-activation (though this might be less critical)
            if (currentStaffCode == request.Request.StaffCode)
            {
                return new ActivateStaffResponse
                {
                    Success = false,
                    Message = "You cannot activate your own account."
                };
            }

            return await _staffService.ActivateStaffAsync(request.Request.StaffCode);
        }
    }
}