using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Threading.Tasks;
using TMFDomain.Interfaces.Authentication;

public class EmailService : IEmailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _emailFrom;
    private readonly string _emailPassword;

    // Property to store the last error message
    public string LastErrorMessage { get; private set; }

    public EmailService()
    {
        _host = "smtp.gmail.com";
        _port = 465; // Use port 465 for SSL
        _emailFrom = "ifeoluwa758@gmail.com";
        _emailPassword = "ihzv dhpt ijsa vddz"; // Ensure this is the correct app-specific password
    }

    public async Task<bool> SendAsync(string to, string subject, string body, bool isHtml = true)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Tasuwab Microfinance Bank", _emailFrom));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;

            // Set the email body (HTML or plain text)
            var bodyBuilder = new BodyBuilder();
            if (isHtml)
            {
                bodyBuilder.HtmlBody = body;
            }
            else
            {
                bodyBuilder.TextBody = body;
            }
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_host, _port, true); // Use SSL
                await client.AuthenticateAsync(_emailFrom, _emailPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }

            return true;
        }
        catch (Exception ex)
        {
            // Store the error message
            LastErrorMessage = ex.Message;
            Console.WriteLine($"Error sending email: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return false;
        }
    }
    //public async Task<bool> SendPasswordResetEmailAsync(string to, string temporaryPassword)
    //{
    //    string subject = "Your Temporary Password";
    //    string body = $"Your temporary password is: {temporaryPassword}";

    //    return await SendAsync(to, subject, body, false);
    //}


    public async Task<bool> SendPasswordResetEmailAsync(string to, string staffCode, string temporaryPassword)
    {
        string subject = "Your Temporary Password";
        string body = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Temporary Password</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
            }}
            .email-container {{
                max-width: 600px;
                margin: 20px auto;
                background-color: #ffffff;
                border-radius: 8px;
                overflow: hidden;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }}
            .email-header {{
                background-color: #007bff;
                color: #ffffff;
                text-align: center;
                padding: 20px;
            }}
            .email-header img {{
                max-width: 150px;
                height: auto;
                margin-bottom: 10px;
            }}
            .email-header h1 {{
                margin: 0;
                font-size: 24px;
            }}
            .email-body {{
                padding: 20px;
                color: #333333;
            }}
            .email-body h2 {{
                font-size: 20px;
                margin-bottom: 20px;
            }}
            .email-body p {{
                font-size: 16px;
                line-height: 1.5;
            }}
            .email-body .password {{
                font-size: 18px;
                font-weight: bold;
                color: #007bff;
                margin: 20px 0;
            }}
            .email-footer {{
                background-color: #f4f4f4;
                text-align: center;
                padding: 15px;
                font-size: 14px;
                color: #666666;
            }}
            .email-footer a {{
                color: #007bff;
                text-decoration: none;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='email-header'>
                <img src='https://tasuwabhomes.com/wp-content/uploads/2024/09/IMG_0173-removebg-preview-white-bg.png' alt='Tasuwab Homes Logo'>
            </div>
            <div class='email-body'>
                <h2>Hello,</h2>
                <p>Your staff code is: <strong>{staffCode}</strong></p>
                <p>Your temporary password has been generated. Please use the following password to log in:</p>
                <div class='password'>{temporaryPassword}</div>
                <p>This password will expire in <strong>24 hours</strong>. Please log in and set a new password immediately.</p>
                <p>If you did not request this password, please contact our support team.</p>
            </div>
            <div class='email-footer'>
                <p>If you have any questions, feel free to <a href='mailto:support@tasuwabhomes.com'>contact us</a>.</p>
                <p>&copy; 2025 Tasuwab Microfinance Bank. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>";

        return await SendAsync(to, subject, body);
    }
}