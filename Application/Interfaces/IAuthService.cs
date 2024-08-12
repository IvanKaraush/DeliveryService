using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUser(User user);
        Task<Restaurant> GetRestaurantByAuth(AuthModel authModel);
        Task<User> GetUserByAuth(AuthModel authModel);
        Task AuthHost(AuthModel authModel);
    }
}
