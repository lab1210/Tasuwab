using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.DTOs.Admin;

namespace TMFApplication.Contracts.Admin.Commands
{
    public class ActivateStaffCommand : IRequest<ActivateStaffResponse>
    {
        public ActivateStaffRequest Request { get; set; }
    }
}