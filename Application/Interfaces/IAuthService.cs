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
        Task RegisterUser(UserRegisterModel userRegisterModel);
        Task<Restaurant> GetRestaurantByAuth(AuthModel authModel);
        Task<UserOutputModel> GetUserByAuth(AuthModel authModel);
        void AuthHost(AuthModel authModel);
    }
}
