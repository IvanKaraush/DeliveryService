using Application.Interfaces;
using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        public AuthService(IUserStore userStore, IRestaurantStore restaurantStore, IOptions<HostAuthOptions> hostAuth) 
        {
            HostAuth = hostAuth.Value;
            UserStore = userStore;
            RestaurantStore = restaurantStore;
        }
        private readonly HostAuthOptions HostAuth;
        private readonly IUserStore UserStore;
        private readonly IRestaurantStore RestaurantStore;
        public async Task AuthHost(AuthModel authModel)
        {
            if (HostAuth.Login != authModel.Login || HostAuth.Password != authModel.Password)
                throw new DoesNotExistException(typeof(User));
        }

        public async Task<Restaurant> GetRestaurantByAuth(AuthModel authModel)
        {
            return await RestaurantStore.GetRestaurantByAuth(authModel);
        }

        public async Task<UserOutputModel> GetUserByAuth(AuthModel authModel)
        {
            User user = await UserStore.GetUserByAuth(authModel);
            return new UserOutputModel(user);
        }

        public async Task RegisterUser(UserRegisterModel userRegisterModel)
        {
            await UserStore.AddUser(userRegisterModel.ToUser());
        }
    }
}
