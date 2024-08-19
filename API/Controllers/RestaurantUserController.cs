using Application.Interfaces;
using Application.Services;
using Domain.Models.Entities.SQLEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("restaurants")]
    [ApiController]
    public class RestaurantUserController : ControllerBase
    {
        public RestaurantUserController(IRestaurantUserService restaurantUserService)
        {
            _restaurantUserService = restaurantUserService;
        }
        private readonly IRestaurantUserService _restaurantUserService;
        [HttpGet]
        [Route("getrestaurants")]
        public async Task<IActionResult> GetRestaurants(string city)
        {
            if (city == null)
                return BadRequest("Arguments are null");
            return Ok(await _restaurantUserService.GetRestaurantsInCityAdresses(city));
        }
    }
}
