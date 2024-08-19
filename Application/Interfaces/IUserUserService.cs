using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserUserService
    {
        public Task<UserOutputModel> GetUserById(Guid id);
        public Task DeleteUser(Guid id);
        public Task EditUserTelegram(Guid id, string newTelegramId);
        public Task AddUserBirthDate(Guid id, DateOnly birthDate);
        public Task EditUserAuth(Guid id, AuthModel newAuth);
    }
}
