using Domain.Models.Entities;
using Domain.Models.VievModels;

namespace Domain.Stores
{
    public interface IRestaurantStore
    {
        void AddRestaurant(Restaurant restaurant);
        void RemoveRestaurant(string adress);
        Task<List<string>> GetRestaurantsInCityAdresses(string city);
        Task<Restaurant> GetRestaurantByAuth(AuthModel authModel);
        void EditRestaurantAuth(string adress, AuthModel authModel);
    }
}
