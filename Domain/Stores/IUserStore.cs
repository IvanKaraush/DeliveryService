using Domain.Models.Entities;
using Domain.Models.VievModels;

namespace Domain.Stores
{
    public interface IUserStore
    {
        void AddUser(User user);
        void RemoveUser(Guid id);
        void EditUserTelegram(Guid id, string newTelegramId);
        void DebitBonuses(Guid id, decimal amount);
        Task<User> GetUserByAuth(AuthModel authModel);
        Task<User> GetUserById(Guid id);
        void EditUserAuth(Guid id, AuthModel newAuth);
        void AssignAsAdmin(Guid id);
        void UnassignAsAdmin(Guid id);
    }
}
