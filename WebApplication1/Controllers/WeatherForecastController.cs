using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISoldProductStore kek)
        {
            _logger = logger;
            Kek = kek;
        }
        ISoldProductStore Kek;
        [Route("hui")]
        [HttpGet]
        public async Task<List<int>> Get(int q)
        {
            return await Kek.GetHotArticleList(q);
        }
        [Route("zalupa")]
        [HttpPost]
        public void Hui(int a)
        {
            SoldProduct sp = new SoldProduct(24, a);
            Kek.AddSoldProduct(sp);
        }
    }
}
