﻿using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;

namespace Domain.Stores
{
    public interface IRestaurantStore
    {
        Task AddRestaurant(Restaurant restaurant);
        Task RemoveRestaurant(string adress);
        Task<List<string>> GetRestaurantsInCityAdresses(string city);
        Task<Restaurant> GetRestaurantByAuth(AuthModel authModel);
        Task EditRestaurantAuth(string adress, AuthModel authModel);
    }
}
