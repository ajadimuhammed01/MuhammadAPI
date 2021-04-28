 using System;
using System.Threading.Tasks;
using SendGrid;
using Microsoft.Extensions.Options;
using techHowdy.API.Helpers;
using techHowdy.API.Email;
using techHowdy.API.Services;
using SendGrid.Helpers.Mail;

namespace techHowdy.API.Services
{
    public class SendGridEmailSender : IEmailSender
    {
       private readonly AppSettings _appSettings;

       public SendGridEmailSender(IOptions<AppSettings> appSettings)
       {
          _appSettings = appSettings.Value;
       }

       public async Task<SendEmailResponse> SendEmailAsync(string userEmail, string emailSubject, string message)
       {
            var apiKey = _appSettings.SendGridKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("muhammedtemitayo@live.com", "MUHAMMAD");
            var subject = emailSubject;
            var to = new EmailAddress(userEmail, "Test");
            var plainTextContent = message;
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return new SendEmailResponse();   
       }
    }
}