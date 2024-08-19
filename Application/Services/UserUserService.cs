using Application.Interfaces;
using Domain.Models.ApplicationModels;
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
    public class UserUserService : IUserUserService
    {
        public UserUserService(IUserStore userStore, IOptions<ServicesOptions> servicesOptions) 
        {
            _userStore = userStore;
            _bonusesForTelegram = servicesOptions.Value.BonusesForTelegram;
            _bonusesForBirthDate = servicesOptions.Value.BonusesForBirthdate;
        }
        private readonly IUserStore _userStore;
        private readonly decimal _bonusesForTelegram;
        private readonly decimal _bonusesForBirthDate;
        public async Task<UserOutputModel> GetUserById(Guid id)
        {
            return new UserOutputModel(await _userStore.GetUserById(id));
        }
        public async Task DeleteUser(Guid id)
        {
            await _userStore.RemoveUser(id);
        }

        public async Task EditUserAuth(Guid id, AuthModel newAuth)
        {
            await _userStore.EditUserAuth(id, newAuth);
        }

        public async Task AddUserBirthDate(Guid id, DateOnly birthDate)
        {
            await _userStore.AddUserBirthDate(id, birthDate);
            await _userStore.DebitBonuses(id, _bonusesForBirthDate);
        }

        public async Task EditUserTelegram(Guid id, string newTelegramId)
        {
            if(!await _userStore.EditUserTelegram(id, newTelegramId))
                await _userStore.DebitBonuses(id, _bonusesForTelegram);
        }
    }
}
