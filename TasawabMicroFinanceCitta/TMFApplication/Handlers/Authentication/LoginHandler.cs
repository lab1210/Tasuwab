using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.Contracts.Users.Commands.Authentication;
using TMFApplication.DTOs.Authentication;
using TMFDomain.Interfaces;

namespace TMFApplication.Handlers.Authentication
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginHandler(IAuthenticationService authenticationService)
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