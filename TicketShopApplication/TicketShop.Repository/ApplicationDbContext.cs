using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.Identity;

namespace TicketShop.Repository
{
    public class ApplicationDbContext : IdentityDbContext<TicketShopApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<TicketInShoppingCart> TicketInShoppingCarts { get; set; }
        public virtual DbSet<TicketInOrder> TicketInOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ticket>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketInShoppingCart>()
                .HasKey(x => new { x.TicketId, x.ShoppingCartId });

            builder.Entity<TicketInShoppingCart>()
                .HasOne(x => x.Ticket)
                .WithMany(x => x.TicketInShoppingCarts)
                .HasForeignKey(x => x.ShoppingCartId);

            builder.Entity<TicketInShoppingCart>()
                .HasOne(x => x.ShoppingCart)
                .WithMany(x => x.TicketInShoppingCarts)
                .HasForeignKey(x => x.TicketId);

            builder.Entity<ShoppingCart>()
                .HasOne<TicketShopApplicationUser>(x => x.Owner)
                .WithOne(x => x.UserCart)
                .HasForeignKey<ShoppingCart>(x => x.OwnerId);

            builder.Entity<TicketInOrder>()
                .HasKey(x => new { x.TicketId, x.OrderId });

            builder.Entity<TicketInOrder>()
                .HasOne(x => x.OrderedTicket)
                .WithMany(x => x.TicketInOrders)
                .HasForeignKey(x => x.OrderId);

            builder.Entity<TicketInOrder>()
                .HasOne(x => x.UserOrder)
                .WithMany(x => x.TicketInOrders)
                .HasForeignKey(x => x.TicketId);
        }
    }
}
