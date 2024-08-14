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
        Task DeleteUser(Guid id);
        Task EditUserTelegram(Guid id, string newTelegramId);
        Task AddUserBirthDate(Guid id, DateOnly birthDate);
        Task<UserOutputModel> GetUserById(Guid id);
        Task EditUserAuth(Guid id, AuthModel newAuth);
    }
}
