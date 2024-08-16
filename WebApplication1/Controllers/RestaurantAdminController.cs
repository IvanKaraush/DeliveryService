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
            RestaurantAdminService = restaurantAdminService;
        }
        private readonly IRestaurantAdminService RestaurantAdminService;
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(Restaurant restaurant)
        {
            await RestaurantAdminService.AddRestaurant(restaurant, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
        [HttpPost]
        [Route("remove")]
        public async Task<IActionResult> Remove(string adress)
        {
            await RestaurantAdminService.RemoveRestaurant(adress, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
        [HttpPatch]
        [Route("editauth")]
        public async Task<IActionResult> EditAuth(string adress, AuthModel authModel)
        {
            await RestaurantAdminService.EditRestaurantAuth(adress, authModel, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
    }
}
