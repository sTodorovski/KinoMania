using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DTO;
using TicketShop.Repository.Interface;
using TicketShop.Services.Interface;

namespace TicketShop.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        private readonly IRepository<EmailMessage> _emailMessageRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<Order> orderRepository, IRepository<TicketInOrder> ticketInOrderRepository, IRepository<EmailMessage> emailMessageRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _ticketInOrderRepository = ticketInOrderRepository;
            _emailMessageRepository = emailMessageRepository;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.UserCart;

            var allTickets = userShoppingCart.TicketInShoppingCarts.ToList();

            var allPrices = allTickets.Select(x => new
            {
                TicketPrice = x.Ticket.Price,
                Quantity = x.Quantity
            }).ToList();

            var totalPrice = 0;

            foreach (var item in allPrices)
            {
                totalPrice += item.Quantity * item.TicketPrice;
            }

            ShoppingCartDto scDto = new ShoppingCartDto
            {
                Tickets = allTickets,
                TotalPrice = totalPrice
            };

            return scDto;
        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                EmailMessage message = new EmailMessage();
                message.MailTo = loggedInUser.Email;
                message.Subject = "Order successfully created!";
                message.Status = false;
                message.Content = "";

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepository.Insert(order);

                List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();

                var result = userShoppingCart.TicketInShoppingCarts.Select(x => new TicketInOrder
                {
                    Id = Guid.NewGuid(),
                    TicketId = x.Ticket.Id,
                    OrderedTicket = x.Ticket,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity = x.Quantity,
                }).ToList();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Your order is completed. The order contains: ");
                var totalPrice = 0.0;

                for(int i = 1; i <= result.Count; i++)
                {
                    var item = result[i];
                    totalPrice += item.Quantity * item.OrderedTicket.Price;
                    sb.AppendLine(i.ToString() + ". " + item.OrderedTicket.MovieName + " with price of: " + item.OrderedTicket.Price + " and quantity of: " + item.Quantity);
                }

                sb.AppendLine("Total Price: " + totalPrice.ToString());

                message.Content = sb.ToString();

                ticketInOrders.AddRange(result);

                foreach (var item in ticketInOrders)
                {
                    this._ticketInOrderRepository.Insert(item);
                }

                loggedInUser.UserCart.TicketInShoppingCarts.Clear();

                this._emailMessageRepository.Insert(message);

                this._userRepository.Update(loggedInUser);

                return true;
            }
            return false;

        }

        bool IShoppingCartService.deleteTicketFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.TicketInShoppingCarts.Where(x => x.TicketId.Equals(id)).FirstOrDefault();

                userShoppingCart.TicketInShoppingCarts.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }
            return false;
        }
    }
}
