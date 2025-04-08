using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.DTOs.Admin;
using TMFApplication.Services.Admin;

namespace TMFApplication.Handlers.Admin
{
    public class GetStaffWithUserQueryHandler : IRequestHandler<GetStaffWithUserQuery, StaffWithUserResponse>
    {
        private readonly StaffService _staffService;

        public GetStaffWithUserQueryHandler(StaffService staffService)
        {
            _staffService = staffService;
        }

        public async Task<StaffWithUserResponse> Handle(GetStaffWithUserQuery request, CancellationToken cancellationToken)
        {
            return await _staffService.GetStaffWithUserAsync(request.StaffCode);
        }
    }
}
