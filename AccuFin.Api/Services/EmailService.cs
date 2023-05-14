﻿using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;

namespace AccuFin.Api.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine("To:" + email);
            Console.WriteLine("Subject:" + subject);
            Console.WriteLine("Mail:" + htmlMessage);

            //TODO:Implement real mailer
        }
    }
}