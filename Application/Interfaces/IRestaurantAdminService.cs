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
        Task AddRestaurant(Restaurant restaurant);
        Task RemoveRestaurant(string adress);
        Task EditRestaurantAuth(string adress, AuthModel authModel);
    }
}
