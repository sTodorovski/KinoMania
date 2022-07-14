using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketShop.Domain.DomainModels
{
    public class Ticket : BaseEntity
    { 
        [Required]
        public string MovieName { get; set; }
        [Required]
        public string MoviePoster { get; set; }
        [Required]
        public string MovieDescription { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Rating { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCarts { get; set; }
        public IEnumerable<TicketInOrder> TicketInOrders { get; set; }
    }
}
