using AutoSale.Domain.Enum;
using AutoSale.Domain.Helpers;
using AutoSale.Domain.Response;
using AutoSale.Service.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace AutoSale.Service.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly AuthMessageSenderOptions _options;

        public EmailService(IOptions<AuthMessageSenderOptions> options)
        {
            _options = options.Value;
        }
        
        public async Task<IResponse<bool>> SendAsync(string emailTo, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_options.Email));
                email.To.Add(MailboxAddress.Parse(emailTo));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_options.Email, _options.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
                
                return new Response<bool>
                {
                    Data = true,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<bool>
                {
                    Data = false,
                    Description = $"[EmailService:SendAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
            
        }
    }
}