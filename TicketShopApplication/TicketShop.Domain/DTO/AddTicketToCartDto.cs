using System;
using TicketShop.Domain.DomainModels;

namespace TicketShop.Domain.DTO
{
    public class AddTicketToCartDto
    {
        public Ticket SelectedTicket { get; set; }
        public Guid TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
