using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("orders")]
    [ApiController]
    [Authorize("RequireRestaurantRole")]
    public class OrderRestaurantController : ControllerBase
    {
        public OrderRestaurantController(IOrderRestaurantService orderRestaurantService) 
        {
            _orderRestaurantService = orderRestaurantService;
        }
        private readonly IOrderRestaurantService _orderRestaurantService;

        [HttpGet]
        [Route("getorder")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _orderRestaurantService.GetOrderById(id);
            return Ok();
        }
        [HttpPost]
        [Route("getlist")]
        public async Task<IActionResult> GetList(int count, Coordinates restaurantCoordinates)
        {
            if (restaurantCoordinates == null)
                return BadRequest("Arguments are null");
            return Ok(await _orderRestaurantService.GetOrdersList(count, restaurantCoordinates));
        }
        [HttpPatch]
        [Route("accept")]
        public async Task<IActionResult> Accept(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _orderRestaurantService.AcceptOrder(id);
            return Ok();
        }
        [HttpPatch]
        [Route("markascooked")]
        public async Task<IActionResult> MarkAsCooked(Guid id, int article, bool wasCookedEarlier)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _orderRestaurantService.RemoveUnitFromList(id, article, wasCookedEarlier);
            return Ok();
        }
        [HttpDelete]
        [Route("closeorder")]
        public async Task<IActionResult> CloseOrder(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _orderRestaurantService.RemoveOrder(id);
            return Ok();
        }
    }
}
