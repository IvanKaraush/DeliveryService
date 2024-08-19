using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRestaurantAdminService
    {
        public Task AddRestaurant(Restaurant restaurant, Guid admin);
        public Task RemoveRestaurant(string adress, Guid admin);
        public Task EditRestaurantAuth(string adress, AuthModel authModel, Guid admin);
    }
}
