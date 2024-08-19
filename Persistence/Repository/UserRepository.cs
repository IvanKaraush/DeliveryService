using Domain.Models.VievModels;
using Domain.Models.ApplicationModels;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Domain.Models.Entities.SQLEntities;
using Microsoft.Extensions.Options;
using Persistence.Exceptions;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Persistence.Repository
{
    public class UserRepository : IUserStore
    {
        public UserRepository(SQLContext context, IUserCacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }
        private readonly SQLContext _context;
        private readonly IUserCacheService _cacheService;

        public async Task<User> GetUserById(Guid id)
        {
            User? user = await _cacheService.Get(id);
            if (user == null)
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            return user;
        }

        public async Task<User> GetUserByAuth(AuthModel authModel)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Login == authModel.Login && u.Password == authModel.Password);
            if (user == null)
                throw new DoesNotExistException(typeof(User));            
            await _cacheService.Save(user);
            return user;
        }

        public async Task<List<string>> GetBirthdayPeopleTelegram()
        {
#pragma warning disable CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
            return await _context.Users.Where(u => u.TelegramId != null && u.BirthDate.HasValue && u.BirthDate.Value == DateOnly.FromDateTime(DateTime.Now)).Select(u => u.TelegramId).ToListAsync();
#pragma warning restore CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await _cacheService.Save(user);
        }

        public async Task RemoveUser(Guid id)
        {
            User? user = await _cacheService.Get(id);
            if (user == null)
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            await _cacheService.Remove(id);
        }

        public async Task<bool> EditUserTelegram(Guid id, string newTelegramId)
        {
            User? user = await _cacheService.Get(id);
            if (user == null)
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            bool hadAlreadyTelegramId = false;
            if (user.TelegramId != null)
                hadAlreadyTelegramId = true;
            user.TelegramId = newTelegramId;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await _cacheService.Update(user);
            return hadAlreadyTelegramId;
        }
        public async Task EditUserAuth(Guid id, AuthModel newAuth)
        {
            User? user = await _cacheService.Get(id);
            if (user == null)
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.Login = newAuth.Login;
            user.Password = newAuth.Password;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await _cacheService.Update(user);
        }
        public async Task AssignAsAdmin(Guid id)
        {
            User? user = await _cacheService.Get(id);
            if (user == null)
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.IsAdmin = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await _cacheService.Update(user);
        }
        public async Task UnassignAsAdmin(Guid id)
        {
            User? user = await _cacheService.Get(id);
            if (user == null)
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.IsAdmin = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await _cacheService.Update(user);
        }

        public async Task DebitBonuses(Guid id, decimal amount)
        {
            User? user = await _cacheService.Get(id);
            if (user == null)
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.Bonuses -= amount;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await _cacheService.Update(user);
        }

        public async Task AddUserBirthDate(Guid id, DateOnly birthDate)
        {
            User? user = await _cacheService.Get(id);
            if (user == null)
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            if (user.BirthDate != null)
                throw new WasAlreadySetException("Birthdate");
            user.BirthDate = birthDate;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await _cacheService.Update(user);
        }
    }
}
