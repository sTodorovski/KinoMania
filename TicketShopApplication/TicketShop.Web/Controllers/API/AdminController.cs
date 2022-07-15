﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.Identity;
using TicketShop.Services.Interface;

namespace TicketShop.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<TicketShopApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminController(IOrderService orderService, UserManager<TicketShopApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._orderService = orderService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        [HttpGet("[action]")]
        public List<Order> GetAllActiveOrders()
        {
            return this._orderService.GetAllOrders();
        }

        [HttpPost("[action]")]
        public Order GetDetailsForOrder(BaseEntity model)
        {
            return this._orderService.GetOrderDetails(model);
        }

        [HttpPost("[action]")]
        public bool ImportAllUsers(List<UserRegistrationDto> model)
        {
            bool status = true;

            foreach (var item in model)
            {
                var userCheck = userManager.FindByEmailAsync(item.Email).Result;
                if(userCheck == null)
                {
                    var user = new TicketShopApplicationUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserCart = new ShoppingCart()
                    };

                    var result = userManager.CreateAsync(user, item.Password).Result;

                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }

            return status;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return null;
        }
    }
}
