using Application.Interfaces;
using Domain.Models.VievModels;
using Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserUserService : IUserUserService
    {
        public UserUserService(IUserStore userStore) 
        {
            UserStore = userStore;
        }
        private readonly IUserStore UserStore;
        public async Task DeleteUser(Guid id)
        {
            await UserStore.RemoveUser(id);
        }

        public async Task EditUserAuth(Guid id, AuthModel newAuth)
        {
            await UserStore.EditUserAuth(id, newAuth);
        }

        public async Task AddUserBirthDate(Guid id, DateOnly birthDate)
        {
            await UserStore.AddUserBirthDate(id, birthDate);
            await UserStore.DebitBonuses(id, 10);
        }

        public async Task EditUserTelegram(Guid id, string newTelegramId)
        {
            if(!await UserStore.EditUserTelegram(id, newTelegramId))
                await UserStore.DebitBonuses(id, 10);
        }

        public async Task<UserOutputModel> GetUserById(Guid id)
        {
            return new UserOutputModel(await UserStore.GetUserById(id));
        }
    }
}
