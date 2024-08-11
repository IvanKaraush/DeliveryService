using Domain.Models.ApplicationModels;
using Domain.Models.Entities;
using Domain.Models.VievModels;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository
{
    public class RestaurantRepository : IRestaurantStore
    {
        public RestaurantRepository(SQLContext context)
        {
            Context = context;
        }
        private readonly SQLContext Context;
        public async void AddRestaurant(Restaurant restaurant)
        {
            await Context.Restaurants.AddAsync(restaurant);
            await Context.SaveChangesAsync();
        }

        public async Task<List<string>> GetRestaurantsInCityAdresses(string city)
        {
            return await Context.Restaurants.Where(r => EF.Functions.Like(r.Adress, "%" + city + "%")).Select(r=>r.Adress).ToListAsync();
        }

        public async void RemoveRestaurant(string adress)
        {
            Restaurant? restaurant = await Context.Restaurants.FirstOrDefaultAsync(r => r.Adress.Equals(adress));
            if (restaurant == null)
                throw new DoesNotExistException(typeof(Restaurant));
            Context.Restaurants.Remove(restaurant);
            await Context.SaveChangesAsync();
        }

        public async Task<Restaurant> GetRestaurantByAuth(AuthModel authModel)
        {
            Restaurant? restaurant = await Context.Restaurants.FirstOrDefaultAsync(r => r.Login == authModel.Login && r.Password == authModel.Password);
            if (restaurant == null)
                throw new DoesNotExistException(typeof(Restaurant));
            return restaurant;
        }

        public async void EditRestaurantAuth(string adress, AuthModel authModel)
        {
            Restaurant? restaurant = await Context.Restaurants.FirstOrDefaultAsync(r => r.Adress.Equals(adress));
            if (restaurant == null)
                throw new DoesNotExistException(typeof(Restaurant));
            restaurant.Login = authModel.Login;
            restaurant.Password = authModel.Password;
            Context.Restaurants.Update(restaurant);
            await Context.SaveChangesAsync();
        }
    }
}
