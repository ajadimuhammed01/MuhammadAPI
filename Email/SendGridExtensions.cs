using System;
using Microsoft.Extensions.DependencyInjection;
using techHowdy.API.Email;
using techHowdy.API.Services;

namespace  techHoowdy.API.Email
{
   public static class SendGridExtensions
   {
       public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services)
       {
           services.AddTransient<IEmailSender, SendGridEmailSender>();

           return services;
       }

   }
              
}