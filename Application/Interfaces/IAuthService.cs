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
        public Task<Restaurant> GetRestaurantByAuth(AuthModel authModel);
        public Task<UserOutputModel> GetUserByAuth(AuthModel authModel);
        public Task RegisterUser(UserRegisterModel userRegisterModel);
        public ValueTask<bool> AuthHost(AuthModel authModel);
    }
}
