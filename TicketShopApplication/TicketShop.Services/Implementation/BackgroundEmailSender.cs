using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketShop.Domain.DomainModels;
using TicketShop.Repository.Interface;
using TicketShop.Services.Interface;

namespace TicketShop.Services.Implementation
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<EmailMessage> _emailRepository;

        public BackgroundEmailSender(IEmailService emailService, IRepository<EmailMessage> emailRepository)
        {
            _emailService = emailService;
            _emailRepository = emailRepository;
        }

        public async Task DoWork()
        {
            await _emailService.SendEmailAsync(_emailRepository.GetAll().Where(x => !x.Status).ToList());
        }
    }
}
