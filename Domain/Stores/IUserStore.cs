using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;

namespace Domain.Stores
{
    public interface IUserStore
    {
        public Task<User> GetUserById(Guid id);
        public Task<User> GetUserByAuth(AuthModel authModel);
        public Task<List<string>> GetBirthdayPeopleTelegram();
        public Task AddUser(User user);
        public Task RemoveUser(Guid id);
        public Task<bool> EditUserTelegram(Guid id, string newTelegramId);
        public Task AddUserBirthDate(Guid id, DateOnly birthDate);
        public Task DebitBonuses(Guid id, decimal amount);
        public Task EditUserAuth(Guid id, AuthModel newAuth);
        public Task AssignAsAdmin(Guid id);
        public Task UnassignAsAdmin(Guid id);
    }
}
