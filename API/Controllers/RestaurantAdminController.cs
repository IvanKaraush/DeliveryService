using Application.Interfaces;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("restaurants")]
    [ApiController]
    [Authorize("RequireAdminRole")]
    public class RestaurantAdminController : ControllerBase
    {
        public RestaurantAdminController(IRestaurantAdminService restaurantAdminService) 
        {
            _restaurantAdminService = restaurantAdminService;
        }
        private readonly IRestaurantAdminService _restaurantAdminService;
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(Restaurant restaurant)
        {
            if (restaurant == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _restaurantAdminService.AddRestaurant(restaurant, userGuid);
            return Ok();
        }
        [HttpPost]
        [Route("remove")]
        public async Task<IActionResult> Remove(string adress)
        {
            if (adress == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _restaurantAdminService.RemoveRestaurant(adress, userGuid);
            return Ok();
        }
        [HttpPatch]
        [Route("editauth")]
        public async Task<IActionResult> EditAuth(string adress, AuthModel authModel)
        {
            if (adress == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _restaurantAdminService.EditRestaurantAuth(adress, authModel, userGuid);
            return Ok();
        }
    }
}
