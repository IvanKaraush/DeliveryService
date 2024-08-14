using Application.Interfaces;
using Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RestaurantUserService : IRestaurantUserService
    {
        public RestaurantUserService(IRestaurantStore restaurantStore) 
        {
            RestaurantStore = restaurantStore;
        }
        private readonly IRestaurantStore RestaurantStore;
        public async Task<List<string>> GetRestaurantsInCityAdresses(string city)
        {
            return await RestaurantStore.GetRestaurantsInCityAdresses(city);
        }
    }
}
