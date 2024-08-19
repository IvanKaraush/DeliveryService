using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Persistence.Exceptions;

namespace Persistence.Repository
{
    public class RestaurantRepository : IRestaurantStore
    {
        public RestaurantRepository(SQLContext context)
        {
            _context = context;
        }
        private readonly SQLContext _context;
        public async Task AddRestaurant(Restaurant restaurant)
        {
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetRestaurantsInCityAdresses(string city)
        {
            return await _context.Restaurants.Where(r => EF.Functions.Like(r.Adress, "%" + city + "%")).Select(r=>r.Adress).ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantByAuth(AuthModel authModel)
        {
            Restaurant? restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Login == authModel.Login && r.Password == authModel.Password);
            if (restaurant == null)
                throw new DoesNotExistException(typeof(Restaurant));
            return restaurant;
        }

        public async Task RemoveRestaurant(string adress)
        {
            Restaurant? restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Adress.Equals(adress));
            if (restaurant == null)
                throw new DoesNotExistException(typeof(Restaurant));
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task EditRestaurantAuth(string adress, AuthModel authModel)
        {
            Restaurant? restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Adress.Equals(adress));
            if (restaurant == null)
                throw new DoesNotExistException(typeof(Restaurant));
            restaurant.Login = authModel.Login;
            restaurant.Password = authModel.Password;
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }
    }
}
