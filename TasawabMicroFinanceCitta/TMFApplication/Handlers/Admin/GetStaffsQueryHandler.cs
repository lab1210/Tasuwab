using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.Contracts.Admin.Queries;
using TMFApplication.DTOs.Admin;
using TMFApplication.Services.Admin;

namespace TMFApplication.Handlers.Admin
{
    public class GetStaffsQueryHandler : IRequestHandler<GetStaffsQuery, GetStaffsResponse>
    {
        private readonly StaffService _staffService;

        public GetStaffsQueryHandler(StaffService staffService)
        {
            _staffService = staffService;
        }

        public async Task<GetStaffsResponse> Handle(GetStaffsQuery request, CancellationToken cancellationToken)
        {
            return await _staffService.GetStaffsAsync();
        }
    }
}