using Application.Interfaces;
using Application.Services;
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
            RestaurantUserService = restaurantUserService;
        }
        private readonly IRestaurantUserService RestaurantUserService;
        [HttpGet]
        [Route("restaurants")]
        public async Task<IActionResult> Restaurants(string city)
        {
            return Ok(RestaurantUserService.GetRestaurantsInCityAdresses(city));
        }
    }
}
