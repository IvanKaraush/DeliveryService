using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;

namespace Domain.Stores
{
    public interface IUserStore
    {
        Task AddUser(User user);
        Task RemoveUser(Guid id);
        Task<bool> EditUserTelegram(Guid id, string newTelegramId);
        Task AddUserBirthDate(Guid id, DateOnly birthDate);
        Task DebitBonuses(Guid id, decimal amount);
        Task<User> GetUserByAuth(AuthModel authModel);
        Task<User> GetUserById(Guid id);
        Task EditUserAuth(Guid id, AuthModel newAuth);
        Task AssignAsAdmin(Guid id);
        Task UnassignAsAdmin(Guid id);
        Task<List<string>> GetBirthdayPeopleTelegram();
    }
}
