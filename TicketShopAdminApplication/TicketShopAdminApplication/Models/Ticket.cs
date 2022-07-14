using System;

namespace TicketShopAdminApplication.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string MoviePoster { get; set; }
        public string MovieDescription { get; set; }
        public string Genre { get; set; }
        public int Price { get; set; }
        public int Rating { get; set; }
    }
}
