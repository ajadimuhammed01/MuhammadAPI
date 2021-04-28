using System;
namespace techHowdy.API.Email
{
    public class SendEmailResponse
    {
        public  bool Successful => ErrorMsg == null;

        public string ErrorMsg {get; set;}
    }
}