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
        public AuthService(IUserStore userStore, IRestaurantStore restaurantStore, IOptions<HostAuthOptions> hostAuth, IOptions<ServicesOptions> servicesOptions)
        {
            _hostAuth = hostAuth.Value;
            _userStore = userStore;
            _restaurantStore = restaurantStore;
            _bonusesForTelegram = servicesOptions.Value.BonusesForTelegram;
            _bonusesForBirthDate = servicesOptions.Value.BonusesForBirthdate;
        }
        private readonly HostAuthOptions _hostAuth;
        private readonly IUserStore _userStore;
        private readonly IRestaurantStore _restaurantStore;
        private readonly decimal _bonusesForTelegram;
        private readonly decimal _bonusesForBirthDate;

        public async Task<Restaurant> GetRestaurantByAuth(AuthModel authModel)
        {
            return await _restaurantStore.GetRestaurantByAuth(authModel);
        }

        public async Task<UserOutputModel> GetUserByAuth(AuthModel authModel)
        {
            User user = await _userStore.GetUserByAuth(authModel);
            return new UserOutputModel(user);
        }
        public ValueTask<bool> AuthHost(AuthModel authModel)
        {
            if (_hostAuth.Login != authModel.Login || _hostAuth.Password != authModel.Password)
                return new ValueTask<bool>(false);
            return new ValueTask<bool>(true);
        }

        public async Task RegisterUser(UserRegisterModel userRegisterModel)
        {
            User user = userRegisterModel.ToUser();
            if (userRegisterModel.BirthDate.HasValue)
                user.Bonuses += _bonusesForBirthDate;
            if (userRegisterModel.TelegramId != null)
                user.Bonuses += _bonusesForTelegram;
            await _userStore.AddUser(user);
        }
    }
}
