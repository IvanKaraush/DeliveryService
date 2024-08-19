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
            _orderUserService = orderUserService;
        }
        private readonly IOrderUserService _orderUserService;

        [HttpGet]
        [Route("Getorder")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            return Ok(await _orderUserService.GetOrderById(id));
        }

        [HttpGet]
        [Route("getuserorders")]
        public async Task<IActionResult> GetUserOrders()
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            return Ok(await _orderUserService.GetOrdersByUserId(userGuid));
        }
        [HttpPost]
        [Route("makeorder")]
        public async Task<IActionResult> MakeOrder(OrderCreateModel orderCreateModel)
        {
            if (orderCreateModel == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            OrderModel orderModel = orderCreateModel.ToOrderModel(userGuid);
            await _orderUserService.AddOrder(orderModel);
            return Ok();
        }

        [HttpDelete]
        [Route("cancelorder")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _orderUserService.RemoveOrder(id);
            return Ok();
        }
    }
}
