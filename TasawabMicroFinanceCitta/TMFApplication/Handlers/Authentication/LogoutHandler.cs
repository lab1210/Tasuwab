//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TMFApplication.Contracts.Users.Commands.Authentication;
//using TMFApplication.DTOs.Authentication;
//using TMFDomain.Interfaces;

//namespace TMFApplication.Handlers.Authentication
//{
//    public class LogoutHandler : IRequestHandler<LogoutCommand, LogoutResponse>
//    {
//        private readonly IAuthenticationService _authenticationService;

//        public LogoutHandler(IAuthenticationService authenticationService)
//        {
//            _authenticationService = authenticationService;
//        }

//        public async Task<LogoutResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
//        {
//            var result = await _authenticationService.LogoutAsync(request.Request.RefreshToken);
//            return new LogoutResponse
//            {
//                Success = result.success,
//                Message = result.message
//            };
//        }
//    }
//}
