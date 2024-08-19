using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;

namespace Domain.Stores
{
    public interface IRestaurantStore
    {
        public Task<Restaurant> GetRestaurantByAuth(AuthModel authModel);
        public Task<List<string>> GetRestaurantsInCityAdresses(string city);
        public Task AddRestaurant(Restaurant restaurant);
        public Task RemoveRestaurant(string adress);
        public Task EditRestaurantAuth(string adress, AuthModel authModel);
    }
}
