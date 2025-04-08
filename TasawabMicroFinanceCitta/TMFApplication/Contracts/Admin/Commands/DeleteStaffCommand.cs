using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.DTOs.Admin;

namespace TMFApplication.Contracts.Admin.Commands
{
    public class DeleteStaffCommand : IRequest<DeleteStaffResponse>
    {
        public DeleteStaffRequest Request { get; set; }
    }
}