using System;
using System.Threading.Tasks;

namespace  techHowdy.API.Email
{
    public interface IEmailSender
    {
        Task <SendEmailResponse> SendEmailAsync(string userEmail, string emailSubject, string message);
    }
}