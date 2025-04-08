using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.Contracts.Admin.Commands;
using TMFApplication.DTOs.Admin;
using TMFApplication.Services.Admin;

namespace TMFApplication.Handlers.Admin
{
    public class CreateStaffCommandHandler : IRequestHandler<CreateStaffCommand, CreateStaffResponse>
    {
        private readonly StaffService _staffService;

        public CreateStaffCommandHandler(StaffService staffService)
        {
            _staffService = staffService;
        }

        public async Task<CreateStaffResponse> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {
            return await _staffService.CreateStaffAsync(request.Request);
        }
    }
}
