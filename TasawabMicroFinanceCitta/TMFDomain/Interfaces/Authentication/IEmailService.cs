using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMFDomain.Interfaces.Authentication
{
    public interface IEmailService
    {
        string LastErrorMessage { get; }
        Task<bool> SendAsync(string to, string subject, string body, bool isHtml = true);
        Task<bool> SendPasswordResetEmailAsync(string to, string staffCode, string temporaryPassword);
    }
}
