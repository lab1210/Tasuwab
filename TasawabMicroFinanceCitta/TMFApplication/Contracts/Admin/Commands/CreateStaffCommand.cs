using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.DTOs.Admin;

namespace TMFApplication.Contracts.Admin.Commands
{
    public class CreateStaffCommand : IRequest<CreateStaffResponse>
    {
        public CreateStaffRequest Request { get; set; }
    }
}
