using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.DTOs.Authentication;
using TMFDomain.Interfaces;

namespace TMFApplication.Contracts.Users.Commands.Authentication
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.LoginAsync(request.Request.EmailOrStaffCode, request.Request.Password);
            return new LoginResponse
            {
                Success = result.success,
                Message = result.message,
                Token = result.token,
            };
        }
    }
}
