using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.DTOs.Authentication;

namespace TMFApplication.Contracts.Users.Commands.Authentication
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public LoginRequest Request { get; set; }
    }
}
