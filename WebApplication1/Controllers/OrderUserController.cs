using Application.Interfaces;
using Application.Services;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("orders")]
    [ApiController]
    [Authorize("RequireUserRole")]
    public class OrderUserController : ControllerBase
    {
        public OrderUserController(IOrderUserService orderUserService) 
        {
            OrderUserService = orderUserService;
        }
        private readonly IOrderUserService OrderUserService;
        [HttpPost]
        [Route("makeorder")]
        public async Task<IActionResult> MakeOrder(OrderCreateModel orderCreateModel)
        {
            OrderModel orderModel = orderCreateModel.ToOrderModel(Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            await OrderUserService.AddOrder(orderModel);
            return Ok();
        }
        [HttpGet]
        [Route("userorders")]
        public async Task<IActionResult> UserOrders()
        {
            return Ok(await OrderUserService.GetOrdersByUserId(Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value)));
        }
        [HttpGet]
        [Route("order")]
        public async Task<IActionResult> Order(Guid id)
        {
            return Ok(await OrderUserService.GetOrderById(id));
        }

        [HttpDelete]
        [Route("cancelorder")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            await OrderUserService.RemoveOrder(id);
            return Ok();
        }
    }
}
