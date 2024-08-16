using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("orders")]
    [ApiController]
    [Authorize("RequireRestaurantRole")]
    public class OrderRestaurantController : ControllerBase
    {
        public OrderRestaurantController(IOrderRestaurantService orderRestaurantService) 
        {
            OrderRestaurantService = orderRestaurantService;
        }
        private readonly IOrderRestaurantService OrderRestaurantService;
        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> List(int count, Coordinates restaurantCoordinates)
        {
            return Ok(await OrderRestaurantService.GetOrdersList(count, restaurantCoordinates));
        }
        [HttpPatch]
        [Route("accept")]
        public async Task<IActionResult> Accept(Guid id)
        {
            await OrderRestaurantService.AcceptOrder(id);
            return Ok();
        }
        [HttpGet]
        [Route("getorder")]
        public async Task<IActionResult> Order(Guid id)
        {
            await OrderRestaurantService.GetOrderById(id);
            return Ok();
        }
        [HttpPatch]
        [Route("markascooked")]
        public async Task<IActionResult> MarkAsCooked(Guid id, int article, bool wasCookedEarlier)
        {
            await OrderRestaurantService.RemoveUnitFromList(id, article, wasCookedEarlier);
            return Ok();
        }
        [HttpDelete]
        [Route("closeorder")]
        public async Task<IActionResult> CloseOrder(Guid id)
        {
            await OrderRestaurantService.RemoveOrder(id);
            return Ok();
        }
    }
}
