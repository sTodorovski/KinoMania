﻿using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TicketShop.Domain;
using TicketShop.Domain.DomainModels;
using TicketShop.Services.Interface;

namespace TicketShop.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(List<EmailMessage> allMails)
        {
            List<MimeMessage> messages = new List<MimeMessage>();

            foreach (var item in allMails)
            {
                var emailMessage = new MimeMessage
                {
                    Sender = new MailboxAddress(_settings.SenderName, _settings.SMTPUserName),
                    Subject = item.Subject
                };

                emailMessage.From.Add(new MailboxAddress(_settings.EmailDisplayName, _settings.SMTPUserName));
                emailMessage.Body = new TextPart(TextFormat.Plain) { Text = item.Content };
                emailMessage.To.Add(new MailboxAddress(item.MailTo, null));
                messages.Add(emailMessage);
            }

            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    var socketOption = _settings.EnableSSL ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
                    await smtp.ConnectAsync(_settings.SMTPServer, _settings.SMTPServerPort, socketOption);

                    if(string.IsNullOrEmpty(_settings.SMTPUserName))
                    {
                        await smtp.AuthenticateAsync(_settings.SMTPUserName, _settings.SMTPPassword);
                    }

                    foreach (var item in messages)
                    {
                        await smtp.SendAsync(item);
                    }

                    await smtp.DisconnectAsync(true);
                }
            }
            catch(SmtpException ex)
            {
                throw ex;
            }
        }
    }
}
